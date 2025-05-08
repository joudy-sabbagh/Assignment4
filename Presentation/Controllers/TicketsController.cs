using System;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Application.UseCases.Tickets;
using Application.Validators;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Presentation.Controllers
{
    public class TicketsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IEventRepository _eventRepo;
        private readonly IAttendeeRepository _attendeeRepo;
        private readonly ILogger<TicketsController> _logger;

        public TicketsController(
            IMediator mediator,
            IEventRepository eventRepo,
            IAttendeeRepository attendeeRepo,
            ILogger<TicketsController> logger)
        {
            _mediator = mediator;
            _eventRepo = eventRepo;
            _attendeeRepo = attendeeRepo;
            _logger = logger;
        }

        public async Task<IActionResult> Index(
            string sortOrder,
            int? eventFilter,
            string categoryFilter)
        {
            _logger.LogInformation("Fetching tickets (sort={Sort}, event={Event}, category={Cat})",
                sortOrder, eventFilter, categoryFilter);

            ViewData["Events"] = await _eventRepo.GetAllAsync();
            ViewData["CategoryList"] = Enum.GetValues(typeof(TicketCategory));

            var list = await _mediator.Send(
                new GetAllTicketsQuery(sortOrder, eventFilter, categoryFilter)
            );

            _logger.LogInformation("Retrieved {Count} tickets", list.Count);
            return View(list);
        }

        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("Rendering Create Ticket form");
            ViewData["Events"] = await _eventRepo.GetAllAsync();
            ViewData["Attendees"] = await _attendeeRepo.GetAllAsync();
            ViewData["CategoryList"] = Enum.GetValues(typeof(TicketCategory));
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTicketDTO dto)
        {
            _logger.LogInformation("Received CreateTicket request for {@Dto}", dto);

            var result = new CreateTicketValidator().Validate(dto);
            if (!result.IsValid)
            {
                _logger.LogWarning("CreateTicketDTO validation failed: {@Errors}", result.Errors);
                result.Errors.ToList().ForEach(err => ModelState.AddModelError(string.Empty, err.ErrorMessage));
                ViewData["Events"] = await _eventRepo.GetAllAsync();
                ViewData["Attendees"] = await _attendeeRepo.GetAllAsync();
                ViewData["CategoryList"] = Enum.GetValues(typeof(TicketCategory));
                return View(dto);
            }

            var ticketId = await _mediator.Send(new CreateTicketCommand(dto));
            _logger.LogInformation("Ticket created with Id {TicketId}", ticketId);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Rendering Edit form for Ticket {Id}", id);

            var item = (await _mediator.Send(new GetAllTicketsQuery("", null, null)))
                       .FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                _logger.LogWarning("Ticket {Id} not found for edit", id);
                return NotFound();
            }

            ViewData["Events"] = await _eventRepo.GetAllAsync();
            ViewData["Attendees"] = await _attendeeRepo.GetAllAsync();
            ViewData["CategoryList"] = Enum.GetValues(typeof(TicketCategory));

            return View(new UpdateTicketDTO
            {
                Id = item.Id,
                EventId = item.EventId,
                AttendeeId = item.AttendeeId,
                TicketType = item.Category,
                Price = item.Price
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateTicketDTO dto)
        {
            _logger.LogInformation("Received UpdateTicket request for {Id}: {@Dto}", id, dto);

            if (id != dto.Id)
            {
                _logger.LogWarning("Route id {RouteId} does not match DTO id {DtoId}", id, dto.Id);
                return NotFound();
            }

            var result = new UpdateTicketValidator().Validate(dto);
            if (!result.IsValid)
            {
                _logger.LogWarning("UpdateTicketDTO validation failed: {@Errors}", result.Errors);
                result.Errors.ToList().ForEach(err => ModelState.AddModelError(string.Empty, err.ErrorMessage));
                ViewData["Events"] = await _eventRepo.GetAllAsync();
                ViewData["Attendees"] = await _attendeeRepo.GetAllAsync();
                ViewData["CategoryList"] = Enum.GetValues(typeof(TicketCategory));
                return View(dto);
            }

            await _mediator.Send(new UpdateTicketCommand(dto));
            _logger.LogInformation("Ticket {Id} updated successfully", id);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Rendering Delete confirmation for Ticket {Id}", id);

            var item = (await _mediator.Send(new GetAllTicketsQuery("", null, null)))
                       .FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                _logger.LogWarning("Ticket {Id} not found for deletion", id);
                return NotFound();
            }

            return View(item);
        }

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
