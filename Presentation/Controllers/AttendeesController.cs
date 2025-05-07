// Presentation/Controllers/AttendeesController.cs
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Application.UseCases.Attendees;
using Application.Validators;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class AttendeesController : Controller
    {
        private readonly IMediator _mediator;

        public AttendeesController(IMediator mediator) => _mediator = mediator;

        // GET: Attendees
        public async Task<IActionResult> Index()
        {
            var model = await _mediator.Send(new GetAllAttendeesQuery());
            return View(model);
        }

        // GET: Attendees/Create
        public IActionResult Create() => View();

        // POST: Attendees/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAttendeeDTO dto)
        {
            var result = new CreateAttendeeValidator().Validate(dto);
            if (!result.IsValid)
            {
                foreach (var err in result.Errors)
                    ModelState.AddModelError(string.Empty, err.ErrorMessage);
                return View(dto);
            }
            await _mediator.Send(new CreateAttendeeCommand(dto));
            return RedirectToAction(nameof(Index));
        }

        // GET: Attendees/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var list = await _mediator.Send(new GetAllAttendeesQuery());
            var item = list.FirstOrDefault(a => a.Id == id);
            if (item == null) return NotFound();

            return View(new UpdateAttendeeDTO
            {
                Id = item.Id,
                Name = item.Name,
                Email = item.Email
            });
        }

        // POST: Attendees/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateAttendeeDTO dto)
        {
            if (id != dto.Id) return NotFound();

            var result = new UpdateAttendeeValidator().Validate(dto);
            if (!result.IsValid)
            {
                foreach (var err in result.Errors)
                    ModelState.AddModelError(string.Empty, err.ErrorMessage);
                return View(dto);
            }

            await _mediator.Send(new UpdateAttendeeCommand(dto));
            return RedirectToAction(nameof(Index));
        }

        // GET: Attendees/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var list = await _mediator.Send(new GetAllAttendeesQuery());
            var item = list.FirstOrDefault(a => a.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        // POST: Attendees/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _mediator.Send(new DeleteAttendeeCommand(id));
            return RedirectToAction(nameof(Index));
        }
    }
}
