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

namespace Presentation.Controllers
{
    public class EventsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IVenueRepository _venueRepo;

        public EventsController(IMediator mediator, IVenueRepository venueRepo)
        {
            _mediator = mediator;
            _venueRepo = venueRepo;
        }

        // GET: Events
        public async Task<IActionResult> Index(string searchString)
        {
            var list = await _mediator.Send(new GetAllEventsQuery());
            if (!string.IsNullOrEmpty(searchString))
                list = list
                    .Where(e => e.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            return View(list);
        }

        // GET: Events/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Venues"] = await _venueRepo.GetAllAsync();
            return View();
        }

        // POST: Events/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEventDTO dto)
        {
            var result = new CreateEventValidator().Validate(dto);
            if (!result.IsValid)
            {
                foreach (var err in result.Errors)
                    ModelState.AddModelError(string.Empty, err.ErrorMessage);
                ViewData["Venues"] = await _venueRepo.GetAllAsync();
                return View(dto);
            }
            await _mediator.Send(new CreateEventCommand(dto));
            return RedirectToAction(nameof(Index));
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var list = await _mediator.Send(new GetAllEventsQuery());
            var item = list.FirstOrDefault(e => e.Id == id);
            if (item == null) return NotFound();

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
            if (id != dto.Id) return NotFound();

            var result = new UpdateEventValidator().Validate(dto);
            if (!result.IsValid)
            {
                foreach (var err in result.Errors)
                    ModelState.AddModelError(string.Empty, err.ErrorMessage);
                ViewData["Venues"] = await _venueRepo.GetAllAsync();
                return View(dto);
            }

            await _mediator.Send(new UpdateEventCommand(dto));
            return RedirectToAction(nameof(Index));
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var list = await _mediator.Send(new GetAllEventsQuery());
            var item = list.FirstOrDefault(e => e.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _mediator.Send(new DeleteEventCommand(id));
            return RedirectToAction(nameof(Index));
        }
    }
}
