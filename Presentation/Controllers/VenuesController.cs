using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common;            
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
            var result = await _mediator.Send(new GetAllVenuesQuery());

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to fetch venues: {Error}", result.Error);
                ModelState.AddModelError(string.Empty, result.Error ?? "Unknown error");
                return View(Enumerable.Empty<VenueListDTO>());
            }

            return View(result.Value);
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

            var validation = new CreateVenueValidator().Validate(dto);
            if (!validation.IsValid)
            {
                _logger.LogWarning("CreateVenueDTO validation failed: {@Errors}", validation.Errors);
                foreach (var err in validation.Errors)
                    ModelState.AddModelError(string.Empty, err.ErrorMessage);
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
            var result = await _mediator.Send(new GetAllVenuesQuery());
            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to fetch venues for edit: {Error}", result.Error);
                return RedirectToAction(nameof(Index));
            }

            var item = result.Value.FirstOrDefault(v => v.Id == id);
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
                return NotFound();

            var validation = new UpdateVenueValidator().Validate(dto);
            if (!validation.IsValid)
            {
                _logger.LogWarning("UpdateVenueDTO validation failed: {@Errors}", validation.Errors);
                foreach (var err in validation.Errors)
                    ModelState.AddModelError(string.Empty, err.ErrorMessage);
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
            var result = await _mediator.Send(new GetAllVenuesQuery());
            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to fetch venues for deletion: {Error}", result.Error);
                return RedirectToAction(nameof(Index));
            }

            var item = result.Value.FirstOrDefault(v => v.Id == id);
            if (item == null)
                return NotFound();

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
