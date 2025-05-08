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

        // GET: Tickets
        public async Task<IActionResult> Index(
            string sortOrder,
            int? eventFilter,
            string categoryFilter)
        {
            _logger.LogInformation(
              "Fetching tickets (sort={Sort}, event={Event}, category={Cat})",
               sortOrder, eventFilter, categoryFilter);

            ViewData["Events"] = await _eventRepo.GetAllAsync();
            ViewData["CategoryList"] = Enum.GetValues(typeof(TicketCategory));

            var result = await _mediator.Send(
                new GetAllTicketsQuery(sortOrder, eventFilter, categoryFilter)
            );

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to fetch tickets: {Error}", result.Error);
                ModelState.AddModelError(string.Empty, result.Error ?? "Unknown error");
                return View(Enumerable.Empty<TicketListDTO>());
            }

            return View(result.Value);
        }

        // GET: Tickets/Create
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("Rendering Create Ticket form");
            ViewData["Events"] = await _eventRepo.GetAllAsync();
            ViewData["Attendees"] = await _attendeeRepo.GetAllAsync();
            ViewData["CategoryList"] = Enum.GetValues(typeof(TicketCategory));
            return View();
        }

        // POST: Tickets/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTicketDTO dto)
        {
            _logger.LogInformation("Received CreateTicket request for {@Dto}", dto);

            var validation = new CreateTicketValidator().Validate(dto);
            if (!validation.IsValid)
            {
                _logger.LogWarning("CreateTicketDTO validation failed: {@Errors}", validation.Errors);
                foreach (var err in validation.Errors)
                    ModelState.AddModelError(string.Empty, err.ErrorMessage);

                ViewData["Events"] = await _eventRepo.GetAllAsync();
                ViewData["Attendees"] = await _attendeeRepo.GetAllAsync();
                ViewData["CategoryList"] = Enum.GetValues(typeof(TicketCategory));
                return View(dto);
            }

            var ticketId = await _mediator.Send(new CreateTicketCommand(dto));
            _logger.LogInformation("Ticket created with Id {TicketId}", ticketId);
            return RedirectToAction(nameof(Index));
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Rendering Edit form for Ticket {Id}", id);
            var result = await _mediator.Send(new GetAllTicketsQuery("", null, null));

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to fetch tickets for edit: {Error}", result.Error);
                return RedirectToAction(nameof(Index));
            }

            var item = result.Value.FirstOrDefault(t => t.Id == id);
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

        // POST: Tickets/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
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
                ViewData["CategoryList"] = Enum.GetValues(typeof(TicketCategory));
                return View(dto);
            }

            await _mediator.Send(new UpdateTicketCommand(dto));
            _logger.LogInformation("Ticket {Id} updated successfully", id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Rendering Delete confirmation for Ticket {Id}", id);
            var result = await _mediator.Send(new GetAllTicketsQuery("", null, null));

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to fetch tickets for deletion: {Error}", result.Error);
                return RedirectToAction(nameof(Index));
            }

            var item = result.Value.FirstOrDefault(t => t.Id == id);
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
