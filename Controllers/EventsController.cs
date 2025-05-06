using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using Application.DTOs;
using Application.UseCases.Events;
using Application.Validators;

namespace MyMVCApp.Controllers
{
    public class EventsController : Controller
    {
        private readonly IEventRepository _eventRepo;
        private readonly IVenueRepository _venueRepo;
        private readonly CreateEventHandler _createEventHandler;
        private readonly GetAllEventsHandler _getAllHandler;
        private readonly EditEventHandler _editEventHandler;
        private readonly DeleteEventHandler _deleteEventHandler;

        public EventsController(
            IEventRepository eventRepo,
            IVenueRepository venueRepo,
            CreateEventHandler createEventHandler,
            GetAllEventsHandler getAllEventsHandler,
            EditEventHandler editEventHandler,
            DeleteEventHandler deleteEventHandler)
        {
            _eventRepo = eventRepo;
            _venueRepo = venueRepo;
            _createEventHandler = createEventHandler;
            _getAllHandler = getAllEventsHandler;
            _editEventHandler = editEventHandler;
            _deleteEventHandler = deleteEventHandler;
        }


        // GET: Events with search
        public async Task<IActionResult> Index(string searchString)
        {
            var events = await _getAllHandler.Handle();

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

            await _createEventHandler.Handle(dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var ev = await _eventRepo.GetByIdAsync(id);
            if (ev == null)
                return NotFound();

            ViewData["Venues"] = await _venueRepo.GetAllAsync();
            return View(ev);
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
                {
                    ModelState.AddModelError(string.Empty, error.ErrorMessage);
                }

                ViewData["Venues"] = await _venueRepo.GetAllAsync();
                return View(dto);
            }

            try
            {
                await _editEventHandler.Handle(dto);
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
            var ev = await _eventRepo.GetByIdWithVenueAsync(id);
            if (ev == null)
                return NotFound();

            return View(ev);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _deleteEventHandler.Handle(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
