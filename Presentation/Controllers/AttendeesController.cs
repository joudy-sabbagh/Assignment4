// Presentation/Controllers/AttendeesController.cs
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Application.Common;            
using Application.DTOs;
using Application.UseCases.Attendees;
using Application.Validators;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Presentation.Controllers
{
    public class AttendeesController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AttendeesController> _logger;

        public AttendeesController(IMediator mediator, ILogger<AttendeesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: Attendees
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Fetching all attendees");
            var result = await _mediator.Send(new GetAllAttendeesQuery());

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to fetch attendees: {Error}", result.Error);
                ModelState.AddModelError(string.Empty, result.Error ?? "Unknown error");
                // pass an empty list to keep the UI intact
                return View(Enumerable.Empty<AttendeeListDTO>());
            }

            return View(result.Value);
        }

        // GET: Attendees/Create
        public IActionResult Create()
        {
            _logger.LogInformation("Rendering Create Attendee form");
            return View();
        }

        // POST: Attendees/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAttendeeDTO dto)
        {
            _logger.LogInformation("Received Create request for {@Dto}", dto);

            var validation = new CreateAttendeeValidator().Validate(dto);
            if (!validation.IsValid)
            {
                _logger.LogWarning("Validation failed: {@Errors}", validation.Errors);
                foreach (var err in validation.Errors)
                    ModelState.AddModelError(string.Empty, err.ErrorMessage);
                return View(dto);
            }

            var newId = await _mediator.Send(new CreateAttendeeCommand(dto));
            _logger.LogInformation("Attendee created with Id {Id}", newId);
            return RedirectToAction(nameof(Index));
        }

        // GET: Attendees/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Rendering Edit form for Attendee {Id}", id);
            var result = await _mediator.Send(new GetAllAttendeesQuery());

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to fetch for edit: {Error}", result.Error);
                return RedirectToAction(nameof(Index));
            }

            var item = result.Value.FirstOrDefault(a => a.Id == id);
            if (item == null)
            {
                _logger.LogWarning("Attendee {Id} not found", id);
                return NotFound();
            }

            return View(new UpdateAttendeeDTO { Id = item.Id, Name = item.Name, Email = item.Email });
        }

        // POST: Attendees/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateAttendeeDTO dto)
        {
            _logger.LogInformation("Received Edit request: {Id}", id);
            if (id != dto.Id) return NotFound();

            var validation = new UpdateAttendeeValidator().Validate(dto);
            if (!validation.IsValid)
            {
                foreach (var err in validation.Errors)
                    ModelState.AddModelError(string.Empty, err.ErrorMessage);
                return View(dto);
            }

            await _mediator.Send(new UpdateAttendeeCommand(dto));
            _logger.LogInformation("Attendee {Id} updated", id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Attendees/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Rendering Delete confirmation for Attendee {Id}", id);
            var result = await _mediator.Send(new GetAllAttendeesQuery());

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to fetch for deletion: {Error}", result.Error);
                return RedirectToAction(nameof(Index));
            }

            var item = result.Value.FirstOrDefault(a => a.Id == id);
            if (item == null) return NotFound();

            return View(item);
        }

        // POST: Attendees/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("Deleting Attendee {Id}", id);
            await _mediator.Send(new DeleteAttendeeCommand(id));
            return RedirectToAction(nameof(Index));
        }
    }
}
