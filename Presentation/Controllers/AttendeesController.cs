// Presentation/Controllers/AttendeesController.cs
using System.Linq;
using System.Threading.Tasks;
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
            var model = await _mediator.Send(new GetAllAttendeesQuery());
            _logger.LogInformation("Retrieved {Count} attendees", model?.Count() ?? 0);
            return View(model);
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

            var result = new CreateAttendeeValidator().Validate(dto);
            if (!result.IsValid)
            {
                _logger.LogWarning("Validation failed: {@Errors}", result.Errors);
                result.Errors.ToList().ForEach(err => ModelState.AddModelError(string.Empty, err.ErrorMessage));
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

            var item = (await _mediator.Send(new GetAllAttendeesQuery())).FirstOrDefault(a => a.Id == id);
            if (item == null)
            {
                _logger.LogWarning("Attendee {Id} not found for edit", id);
                return NotFound();
            }

            return View(new UpdateAttendeeDTO { Id = item.Id, Name = item.Name, Email = item.Email });
        }

        // POST: Attendees/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateAttendeeDTO dto)
        {
            _logger.LogInformation("Received Edit request for Attendee {Id}: {@Dto}", id, dto);

            if (id != dto.Id)
            {
                _logger.LogWarning("Route id {RouteId} does not match DTO id {DtoId}", id, dto.Id);
                return NotFound();
            }

            var result = new UpdateAttendeeValidator().Validate(dto);
            if (!result.IsValid)
            {
                _logger.LogWarning("Validation failed: {@Errors}", result.Errors);
                result.Errors.ToList().ForEach(err => ModelState.AddModelError(string.Empty, err.ErrorMessage));
                return View(dto);
            }

            await _mediator.Send(new UpdateAttendeeCommand(dto));
            _logger.LogInformation("Attendee {Id} updated successfully", id);

            return RedirectToAction(nameof(Index));
        }

        // GET: Attendees/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Rendering Delete confirmation for Attendee {Id}", id);

            var item = (await _mediator.Send(new GetAllAttendeesQuery())).FirstOrDefault(a => a.Id == id);
            if (item == null)
            {
                _logger.LogWarning("Attendee {Id} not found for deletion", id);
                return NotFound();
            }

            return View(item);
        }

        // POST: Attendees/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("Deleting Attendee {Id}", id);
            await _mediator.Send(new DeleteAttendeeCommand(id));
            _logger.LogInformation("Attendee {Id} deleted", id);
            return RedirectToAction(nameof(Index));
        }
    }
}
