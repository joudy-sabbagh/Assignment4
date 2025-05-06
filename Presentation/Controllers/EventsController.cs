using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Validators;
using Application.UseCases.Events;
using Domain.Interfaces;
using MediatR;

namespace MyMVCApp.Controllers
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

        // GET: Events with search
        public async Task<IActionResult> Index(string searchString)
        {
            var events = await _mediator.Send(new GetAllEventsQuery());

            if (!string.IsNullOrEmpty(searchString))
            {
                events = events.Where(e => e.Name.ToLower().Contains(searchString.ToLower())).ToList();
            }

            return View(events);
        }

        // GET: Events/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Venues"] = await _venueRepo.GetAllAsync();
            return View();
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEventDTO dto)
        {
            var validator = new CreateEventValidator();
            var result = validator.Validate(dto);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.ErrorMessage);
                }

                ViewData["Venues"] = await _venueRepo.GetAllAsync();
                return View(dto);
            }

            await _mediator.Send(new CreateEventCommand(dto));
            return RedirectToAction(nameof(Index));
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var allEvents = await _mediator.Send(new GetAllEventsQuery());
            var ev = allEvents.FirstOrDefault(e => e.EventId == id);

            if (ev == null)
                return NotFound();

            var dto = new UpdateEventDTO
            {
                EventId = ev.EventId,
                Name = ev.Name,
                EventDate = ev.EventDate,  
                NormalPrice = ev.NormalPrice,
                VIPPrice = ev.VIPPrice,    
                BackstagePrice = ev.BackstagePrice,
                VenueId = ev.VenueId
            };


            ViewData["Venues"] = await _venueRepo.GetAllAsync();
            return View(dto);
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateEventDTO dto)
        {
            if (id != dto.EventId)
                return NotFound();

            var validator = new UpdateEventValidator();
            var result = validator.Validate(dto);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.ErrorMessage);

                ViewData["Venues"] = await _venueRepo.GetAllAsync();
                return View(dto);
            }

            try
            {
                await _mediator.Send(new UpdateEventCommand(dto));
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Update failed.");
                ViewData["Venues"] = await _venueRepo.GetAllAsync();
                return View(dto);
            }
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var allEvents = await _mediator.Send(new GetAllEventsQuery());
            var ev = allEvents.FirstOrDefault(e => e.EventId == id);

            if (ev == null)
                return NotFound();

            return View(ev);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _mediator.Send(new DeleteEventCommand(id));
            return RedirectToAction(nameof(Index));
        }
    }
}
