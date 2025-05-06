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

        public EventsController(IEventRepository eventRepo, IVenueRepository venueRepo, CreateEventHandler createEventHandler)
        {
            _eventRepo = eventRepo;
            _venueRepo = venueRepo;
            _createEventHandler = createEventHandler;
        }

        // GET: Events with search
        public async Task<IActionResult> Index(string searchString)
        {
            var events = await _eventRepo.GetAllWithVenueAsync();

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
        public async Task<IActionResult> Edit(int id, Event ev)
        {
            if (id != ev.EventId)
                return NotFound();

            try
            {
                await _eventRepo.UpdateAsync(ev);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Update failed.");
                ViewData["Venues"] = await _venueRepo.GetAllAsync();
                return View(ev);
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
            await _eventRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
