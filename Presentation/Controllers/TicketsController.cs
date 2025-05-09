using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Application.Common;
using Application.DTOs;
using Application.UseCases.Tickets;
using Application.Validators;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Application.Interfaces;
using Presentation.Models;

namespace Presentation.Controllers
{
    public class TicketsController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly IMediator _mediator;
        private readonly IEventRepository _eventRepo;
        private readonly IAttendeeRepository _attendeeRepo;
        private readonly ILogger<TicketsController> _logger;

        public TicketsController(
            IMediator mediator,
            IEventRepository eventRepo,
            IAttendeeRepository attendeeRepo,
            ILogger<TicketsController> logger,
            IEmailService emailService)
        {
            _mediator = mediator;
            _eventRepo = eventRepo;
            _attendeeRepo = attendeeRepo;
            _logger = logger;
            _emailService = emailService;
        }

        // GET: Tickets
        public async Task<IActionResult> Index(
            string sortOrder,
            int? eventFilter,
            string categoryFilter,
            int page = 1)
        {
            _logger.LogInformation(
                "Fetching tickets (sort={Sort}, event={Event}, category={Cat}, page={Page})",
                sortOrder, eventFilter, categoryFilter, page);

            ViewData["Events"] = await _eventRepo.GetAllAsync();
            ViewData["CategoryList"] = Enum.GetValues(typeof(TicketCategory))
                                               .Cast<TicketCategory>();
            ViewData["CurrentSort"] = sortOrder;
            ViewData["SelectedEvent"] = eventFilter;
            ViewData["SelectedCategory"] = categoryFilter;

            var result = await _mediator.Send(
                new GetAllTicketsQuery(sortOrder, eventFilter, categoryFilter)
            );

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to fetch tickets: {Error}", result.Error);
                ModelState.AddModelError(string.Empty, result.Error ?? "Unknown error");

                var emptyVm = new PagedListViewModel<TicketListDTO>
                {
                    Items = Enumerable.Empty<TicketListDTO>(),
                    PageNumber = page,
                    PageSize = 20,
                    TotalCount = 0
                };
                return View(emptyVm);
            }

            var all = result.Value!;
            const int PageSize = 20;
            var totalCount = all.Count();
            var items = all
                                .Skip((page - 1) * PageSize)
                                .Take(PageSize)
                                .ToList();

            var vm = new PagedListViewModel<TicketListDTO>
            {
                Items = items,
                PageNumber = page,
                PageSize = PageSize,
                TotalCount = totalCount
            };

            return View(vm);
        }

        // GET: Tickets/Create
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("Rendering Create Ticket form");
            ViewData["Events"] = await _eventRepo.GetAllAsync();
            ViewData["Attendees"] = await _attendeeRepo.GetAllAsync();
            ViewData["CategoryList"] = Enum.GetValues(typeof(TicketCategory))
                                           .Cast<TicketCategory>();
            return View();
        }

        // POST: Tickets/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTicketDTO dto)
        {
            _logger.LogInformation("Received CreateTicket request for {@Dto}", dto);

            var validationResult = new CreateTicketValidator().Validate(dto);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning(
                    "CreateTicketDTO validation failed: {@Errors}",
                    validationResult.Errors);
                foreach (var error in validationResult.Errors)
                    ModelState.AddModelError(string.Empty, error.ErrorMessage);

                ViewData["Events"] = await _eventRepo.GetAllAsync();
                ViewData["Attendees"] = await _attendeeRepo.GetAllAsync();
                ViewData["CategoryList"] = Enum.GetValues(typeof(TicketCategory))
                                               .Cast<TicketCategory>();
                return View(dto);
            }

            var ticketId = await _mediator.Send(new CreateTicketCommand(dto));
            _logger.LogInformation("Ticket created with Id {TicketId}", ticketId);

            var attendee = await _attendeeRepo.GetByIdAsync(dto.AttendeeId);
            var ev = await _eventRepo.GetByIdAsync(dto.EventId);

            if (attendee != null && ev != null)
            {
                var name = attendee.Name;
                var email = attendee.Email.Value;
                var eventName = ev.Name;
                var ticketTier = dto.TicketType.ToString();

                var plainTextContent = $@"
Hello {name},

Thank you for booking with us!

Event       : {eventName}
Ticket Tier : {ticketTier}
Ticket ID   : {ticketId}

We look forward to seeing you there.

Best regards,
Event Manager Team
";

                var htmlContent = $@"
<p>Hello <strong>{name}</strong>,</p>
<p>Thank you for booking with us! Here are your details:</p>
<ul>
  <li><strong>Event:</strong> {eventName}</li>
  <li><strong>Ticket Tier:</strong> {ticketTier}</li>
  <li><strong>Ticket ID:</strong> {ticketId}</li>
</ul>
<p>Best regards,<br/><strong>Event Manager Team</strong></p>
";

                await _emailService.SendEmailAsync(
                    toEmail: email,
                    subject: $"Your {eventName} Ticket Confirmation",
                    plainTextContent: plainTextContent,
                    htmlContent: htmlContent
                );
            }
            else
            {
                _logger.LogWarning(
                    "Could not send email: attendee or event not found (Attendee={AttId}, Event={EvtId})",
                    dto.AttendeeId, dto.EventId
                );
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Rendering Edit form for Ticket {Id}", id);
            var result = await _mediator.Send(
                new GetAllTicketsQuery("", null, null)
            );

            if (!result.IsSuccess)
            {
                _logger.LogWarning(
                    "Failed to fetch tickets for edit: {Error}",
                    result.Error);
                return RedirectToAction(nameof(Index));
            }

            var item = result.Value!.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                _logger.LogWarning("Ticket {Id} not found for edit", id);
                return NotFound();
            }

            ViewData["Events"] = await _eventRepo.GetAllAsync();
            ViewData["Attendees"] = await _attendeeRepo.GetAllAsync();
            ViewData["CategoryList"] = Enum.GetValues(typeof(TicketCategory))
                                           .Cast<TicketCategory>();

            return View(new UpdateTicketDTO
            {
                Id = item.Id,
                EventId = item.EventId,
                AttendeeId = item.AttendeeId,
                TicketType = item.Category,
                Price = item.Price
            });
        }

        // POST: Tickets/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateTicketDTO dto)
        {
            _logger.LogInformation(
                "Received UpdateTicket request for {Id}: {@Dto}",
                id, dto);

            if (id != dto.Id)
                return NotFound();

            var validation = new UpdateTicketValidator().Validate(dto);
            if (!validation.IsValid)
            {
                foreach (var err in validation.Errors)
                    ModelState.AddModelError(string.Empty, err.ErrorMessage);

                ViewData["Events"] = await _eventRepo.GetAllAsync();
                ViewData["Attendees"] = await _attendeeRepo.GetAllAsync();
                ViewData["CategoryList"] = Enum.GetValues(typeof(TicketCategory))
                                               .Cast<TicketCategory>();
                return View(dto);
            }

            await _mediator.Send(new UpdateTicketCommand(dto));
            _logger.LogInformation("Ticket {Id} updated successfully", id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation(
                "Rendering Delete confirmation for Ticket {Id}", id);
            var result = await _mediator.Send(
                new GetAllTicketsQuery("", null, null)
            );

            if (!result.IsSuccess)
            {
                _logger.LogWarning(
                    "Failed to fetch tickets for deletion: {Error}",
                    result.Error);
                return RedirectToAction(nameof(Index));
            }

            var item = result.Value!.FirstOrDefault(t => t.Id == id);
            if (item == null)
                return NotFound();

            return View(item);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("Deleting Ticket {Id}", id);
            await _mediator.Send(new DeleteTicketCommand(id));
            _logger.LogInformation("Ticket {Id} deleted", id);
            return RedirectToAction(nameof(Index));
        }
    }
}
