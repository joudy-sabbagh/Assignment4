// Presentation/Controllers/EventsController.cs
using System;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Application.UseCases.Events;
using Application.Validators;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Presentation.Controllers
{
    public class EventsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IVenueRepository _venueRepo;
        private readonly ILogger<EventsController> _logger;

        public EventsController(
            IMediator mediator,
            IVenueRepository venueRepo,
            ILogger<EventsController> logger)
        {
            _mediator = mediator;
            _venueRepo = venueRepo;
            _logger = logger;
        }

        // GET: Events
        public async Task<IActionResult> Index(string searchString)
        {
            _logger.LogInformation("Fetching all events");
            var list = await _mediator.Send(new GetAllEventsQuery());
            _logger.LogInformation("Retrieved {Count} events", list.Count);

            if (!string.IsNullOrEmpty(searchString))
            {
                _logger.LogInformation("Filtering events by '{Filter}'", searchString);
                list = list
                    .Where(e => e.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                _logger.LogInformation("{Count} events match filter", list.Count);
            }

            return View(list);
        }

        // GET: Events/Create
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("Rendering Create Event form");
            ViewData["Venues"] = await _venueRepo.GetAllAsync();
            return View();
        }

        // POST: Events/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEventDTO dto)
        {
            _logger.LogInformation("Received CreateEvent request for {@Dto}", dto);

            var result = new CreateEventValidator().Validate(dto);
            if (!result.IsValid)
            {
                _logger.LogWarning("CreateEventDTO validation failed: {@Errors}", result.Errors);
                result.Errors.ToList().ForEach(err => ModelState.AddModelError(string.Empty, err.ErrorMessage));
                ViewData["Venues"] = await _venueRepo.GetAllAsync();
                return View(dto);
            }

            var id = await _mediator.Send(new CreateEventCommand(dto));
            _logger.LogInformation("Event created with Id {EventId}", id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Rendering Edit form for Event {Id}", id);
            var item = (await _mediator.Send(new GetAllEventsQuery()))
                           .FirstOrDefault(e => e.Id == id);
            if (item == null)
            {
                _logger.LogWarning("Event {Id} not found for edit", id);
                return NotFound();
            }

            ViewData["Venues"] = await _venueRepo.GetAllAsync();
            return View(new UpdateEventDTO
            {
                Id = item.Id,
                Name = item.Name,
                EventDate = item.EventDate,
                NormalPrice = item.NormalPrice,
                VIPPrice = item.VIPPrice,
                BackstagePrice = item.BackstagePrice,
                VenueId = item.VenueId
            });
        }

        // POST: Events/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateEventDTO dto)
        {
            _logger.LogInformation("Received UpdateEvent request for {Id}: {@Dto}", id, dto);
            if (id != dto.Id)
            {
                _logger.LogWarning("Route id {RouteId} does not match DTO id {DtoId}", id, dto.Id);
                return NotFound();
            }

            var result = new UpdateEventValidator().Validate(dto);
            if (!result.IsValid)
            {
                _logger.LogWarning("UpdateEventDTO validation failed: {@Errors}", result.Errors);
                result.Errors.ToList().ForEach(err => ModelState.AddModelError(string.Empty, err.ErrorMessage));
                ViewData["Venues"] = await _venueRepo.GetAllAsync();
                return View(dto);
            }

            await _mediator.Send(new UpdateEventCommand(dto));
            _logger.LogInformation("Event {Id} updated successfully", id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Rendering Delete confirmation for Event {Id}", id);
            var item = (await _mediator.Send(new GetAllEventsQuery()))
                           .FirstOrDefault(e => e.Id == id);
            if (item == null)
            {
                _logger.LogWarning("Event {Id} not found for deletion", id);
                return NotFound();
            }
            return View(item);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("Deleting event {Id}", id);
            await _mediator.Send(new DeleteEventCommand(id));
            _logger.LogInformation("Event {Id} deleted", id);
            return RedirectToAction(nameof(Index));
        }
    }
}
