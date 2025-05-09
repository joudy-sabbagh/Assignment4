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
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Presentation.Models;

namespace Presentation.Controllers
{
    // Require any authenticated user
    [Authorize]
    public class TicketsController : Controller
    {
        private const string CacheKey = "AllTickets";

        private readonly IMediator _mediator;
        private readonly IEventRepository _eventRepo;
        private readonly IAttendeeRepository _attendeeRepo;
        private readonly IEmailService _emailService;
        private readonly ILogger<TicketsController> _logger;
        private readonly IMemoryCache _cache;

        public TicketsController(
            IMediator mediator,
            IEventRepository eventRepo,
            IAttendeeRepository attendeeRepo,
            ILogger<TicketsController> logger,
            IEmailService emailService,
            IMemoryCache cache)
        {
            _mediator = mediator;
            _eventRepo = eventRepo;
            _attendeeRepo = attendeeRepo;
            _logger = logger;
            _emailService = emailService;
            _cache = cache;
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

            bool useCache = string.IsNullOrEmpty(sortOrder)
                            && eventFilter == null
                            && string.IsNullOrEmpty(categoryFilter);

            List<TicketListDTO> all;
            if (useCache && _cache.TryGetValue(CacheKey, out all))
            {
                // cached
            }
            else
            {
                var result = await _mediator.Send(
                    new GetAllTicketsQuery(sortOrder, eventFilter, categoryFilter)
                );

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to fetch tickets: {Error}", result.Error);
                    ModelState.AddModelError(string.Empty, result.Error ?? "Unknown error");
                    return View(new PagedListViewModel<TicketListDTO>());
                }

                all = result.Value!.ToList();
                if (useCache)
                    _cache.Set(CacheKey, all, TimeSpan.FromMinutes(5));
            }

            const int PageSize = 20;
            var totalCount = all.Count;
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

        // Only Admins may create tickets
        [Authorize(Roles = "Admin")]
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

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        // POST: Tickets/Create
        public async Task<IActionResult> Create(CreateTicketDTO dto)
        {
            _logger.LogInformation("Received CreateTicket request for {@Dto}", dto);

            var validation = new CreateTicketValidator().Validate(dto);
            if (!validation.IsValid)
            {
                _logger.LogWarning("Validation failed: {@Errors}", validation.Errors);
                foreach (var err in validation.Errors)
                    ModelState.AddModelError(string.Empty, err.ErrorMessage);

                ViewData["Events"] = await _eventRepo.GetAllAsync();
                ViewData["Attendees"] = await _attendeeRepo.GetAllAsync();
                ViewData["CategoryList"] = Enum.GetValues(typeof(TicketCategory))
                                               .Cast<TicketCategory>();
                return View(dto);
            }

            var ticketId = await _mediator.Send(new CreateTicketCommand(dto));
            _logger.LogInformation("Ticket created with Id {TicketId}", ticketId);

            _cache.Remove(CacheKey);

            // send confirmation email
            var attendee = await _attendeeRepo.GetByIdAsync(dto.AttendeeId);
            var ev = await _eventRepo.GetByIdAsync(dto.EventId);
            if (attendee != null && ev != null)
            {
                var textContent = $@"Hello {attendee.Name},

Thank you for booking with us!

Event       : {ev.Name}
Ticket Tier : {dto.TicketType}
Ticket ID   : {ticketId}

Best regards,
Event Manager Team
";
                await _emailService.SendEmailAsync(
                    toEmail: attendee.Email.Value,
                    subject: $"Your {ev.Name} Ticket Confirmation",
                    plainTextContent: textContent,
                    htmlContent: null
                );
            }

            return RedirectToAction(nameof(Index));
        }

        // Only Admins may edit tickets
        [Authorize(Roles = "Admin")]
        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Rendering Edit form for Ticket {Id}", id);

            var result = await _mediator.Send(
                new GetAllTicketsQuery("", null, null)
            );
            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to fetch tickets for edit: {Error}", result.Error);
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
                TicketType = item.Category
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        // POST: Tickets/Edit/5
        public async Task<IActionResult> Edit(int id, UpdateTicketDTO dto)
        {
            _logger.LogInformation("Received UpdateTicket request for {Id}: {@Dto}", id, dto);

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

            _cache.Remove(CacheKey);
            return RedirectToAction(nameof(Index));
        }

        // Only Admins may delete tickets
        [Authorize(Roles = "Admin")]
        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Rendering Delete confirmation for Ticket {Id}", id);

            var result = await _mediator.Send(
                new GetAllTicketsQuery("", null, null)
            );
            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to fetch tickets for deletion: {Error}", result.Error);
                return RedirectToAction(nameof(Index));
            }

            var item = result.Value!.FirstOrDefault(t => t.Id == id);
            if (item == null)
                return NotFound();

            return View(item);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        // POST: Tickets/Delete/5
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("Deleting Ticket {Id}", id);
            await _mediator.Send(new DeleteTicketCommand(id));
            _logger.LogInformation("Ticket {Id} deleted", id);

            _cache.Remove(CacheKey);
            return RedirectToAction(nameof(Index));
        }
    }
}
