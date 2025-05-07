// Presentation/Controllers/VenuesController.cs
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Application.UseCases.Venues;
using Application.Validators;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Presentation.Controllers
{
    public class VenuesController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<VenuesController> _logger;

        public VenuesController(IMediator mediator, ILogger<VenuesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: Venues
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Fetching all venues");
            var list = await _mediator.Send(new GetAllVenuesQuery());
            _logger.LogInformation("Retrieved {Count} venues", list.Count);
            return View(list);
        }

        // GET: Venues/Create
        public IActionResult Create()
        {
            _logger.LogInformation("Rendering Create Venue form");
            return View();
        }

        // POST: Venues/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateVenueDTO dto)
        {
            _logger.LogInformation("Received CreateVenue request for {@Dto}", dto);

            var result = new CreateVenueValidator().Validate(dto);
            if (!result.IsValid)
            {
                _logger.LogWarning("CreateVenueDTO validation failed: {@Errors}", result.Errors);
                result.Errors.ToList().ForEach(e => ModelState.AddModelError(string.Empty, e.ErrorMessage));
                return View(dto);
            }

            var newId = await _mediator.Send(new CreateVenueCommand(dto));
            _logger.LogInformation("Venue created with Id {VenueId}", newId);
            return RedirectToAction(nameof(Index));
        }

        // GET: Venues/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Rendering Edit form for Venue {Id}", id);

            var item = (await _mediator.Send(new GetAllVenuesQuery()))
                       .FirstOrDefault(v => v.Id == id);
            if (item == null)
            {
                _logger.LogWarning("Venue {Id} not found for edit", id);
                return NotFound();
            }

            return View(new UpdateVenueDTO
            {
                Id = item.Id,
                Name = item.Name,
                Capacity = item.Capacity,
                Location = item.Location
            });
        }

        // POST: Venues/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateVenueDTO dto)
        {
            _logger.LogInformation("Received UpdateVenue request for {Id}: {@Dto}", id, dto);

            if (id != dto.Id)
            {
                _logger.LogWarning("Route id {RouteId} does not match DTO id {DtoId}", id, dto.Id);
                return NotFound();
            }

            var result = new UpdateVenueValidator().Validate(dto);
            if (!result.IsValid)
            {
                _logger.LogWarning("UpdateVenueDTO validation failed: {@Errors}", result.Errors);
                result.Errors.ToList().ForEach(e => ModelState.AddModelError(string.Empty, e.ErrorMessage));
                return View(dto);
            }

            await _mediator.Send(new UpdateVenueCommand(dto));
            _logger.LogInformation("Venue {Id} updated successfully", id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Venues/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Rendering Delete confirmation for Venue {Id}", id);

            var item = (await _mediator.Send(new GetAllVenuesQuery()))
                       .FirstOrDefault(v => v.Id == id);
            if (item == null)
            {
                _logger.LogWarning("Venue {Id} not found for deletion", id);
                return NotFound();
            }

            return View(item);
        }

        // POST: Venues/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("Deleting venue {Id}", id);
            await _mediator.Send(new DeleteVenueCommand(id));
            _logger.LogInformation("Venue {Id} deleted", id);
            return RedirectToAction(nameof(Index));
        }
    }
}
