using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.UseCases.Attendees;
using Application.Validators;
using MediatR;

namespace MyMVCApp.Controllers
{
    public class AttendeesController : Controller
    {
        private readonly IMediator _mediator;

        public AttendeesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: Attendees
        public async Task<IActionResult> Index()
        {
            var attendees = await _mediator.Send(new GetAllAttendeesQuery());
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

            await _mediator.Send(new CreateAttendeeCommand(dto));
            return RedirectToAction(nameof(Index));
        }

        // GET: Attendees/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var attendees = await _mediator.Send(new GetAllAttendeesQuery());
            var attendee = attendees.FirstOrDefault(a => a.Id == id);

            if (attendee == null)
                return NotFound();

            var dto = new UpdateAttendeeDTO
            {
                AttendeeId = attendee.Id,
                Name = attendee.Name,
                Email = attendee.Email
            };

            return View(dto);
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

            await _mediator.Send(new UpdateAttendeeCommand(dto));
            return RedirectToAction(nameof(Index));
        }

        // GET: Attendees/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var attendees = await _mediator.Send(new GetAllAttendeesQuery());
            var attendee = attendees.FirstOrDefault(a => a.Id == id);

            if (attendee == null)
                return NotFound();

            return View(attendee);
        }

        // POST: Attendees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _mediator.Send(new DeleteAttendeeCommand(id));
            return RedirectToAction(nameof(Index));
        }
    }
}