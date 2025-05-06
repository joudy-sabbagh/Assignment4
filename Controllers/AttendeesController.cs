using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using Application.DTOs;
using Application.UseCases.Attendees;
using Application.Validators;

namespace MyMVCApp.Controllers
{
    public class AttendeesController : Controller
    {
        private readonly IAttendeeRepository _attendeeRepo;
        private readonly CreateAttendeeHandler _createHandler;
        private readonly UpdateAttendeeHandler _updateHandler;
        private readonly DeleteAttendeeHandler _deleteHandler;
        private readonly GetAllAttendeesHandler _getAllHandler;

        public AttendeesController(
            IAttendeeRepository attendeeRepo,
            CreateAttendeeHandler createHandler,
            UpdateAttendeeHandler updateHandler,
            DeleteAttendeeHandler deleteHandler,
            GetAllAttendeesHandler getAllHandler)
        {
            _attendeeRepo = attendeeRepo;
            _createHandler = createHandler;
            _updateHandler = updateHandler;
            _deleteHandler = deleteHandler;
            _getAllHandler = getAllHandler;
        }

        // GET: Attendees
        public async Task<IActionResult> Index()
        {
            var attendees = await _getAllHandler.Handle();
            return View(attendees);
        }

        // GET: Attendees/Create
        public IActionResult Create() => View();

        // POST: Attendees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAttendeeDTO dto)
        {
            var validator = new CreateAttendeeValidator();
            var result = validator.Validate(dto);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.ErrorMessage);

                return View(dto);
            }

            await _createHandler.Handle(dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: Attendees/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var attendee = await _attendeeRepo.GetByIdAsync(id);
            if (attendee == null)
                return NotFound();
            return View(attendee);
        }

        // POST: Attendees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateAttendeeDTO dto)
        {
            if (id != dto.AttendeeId)
                return NotFound();

            var validator = new UpdateAttendeeValidator();
            var result = validator.Validate(dto);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.ErrorMessage);

                return View(dto);
            }

            await _updateHandler.Handle(dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: Attendees/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var attendee = await _attendeeRepo.GetByIdAsync(id);
            if (attendee == null)
                return NotFound();
            return View(attendee);
        }

        // POST: Attendees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _deleteHandler.Handle(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
