using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Validators;
using Application.UseCases.Venues;
using Domain.Interfaces;
using MediatR;

namespace Presentation.Controllers
{
    public class VenuesController : Controller
    {
        private readonly IMediator _mediator;

        public VenuesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: Venues
        public async Task<IActionResult> Index()
        {
            var venues = await _mediator.Send(new GetAllVenuesQuery());
            return View(venues);
        }

        // GET: Venues/Create
        public IActionResult Create() => View();

        // POST: Venues/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateVenueDTO dto)
        {
            var validator = new CreateVenueValidator();
            var result = validator.Validate(dto);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.ErrorMessage);

                return View(dto);
            }

            await _mediator.Send(new CreateVenueCommand(dto));
            return RedirectToAction(nameof(Index));
        }

        // GET: Venues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var venues = await _mediator.Send(new GetAllVenuesQuery());
            var venue = venues.FirstOrDefault(v => v.VenueId == id);
            if (venue == null) return NotFound();

            var dto = new UpdateVenueDTO
            {
                VenueId = venue.VenueId,
                Name = venue.Name,
                Capacity = venue.Capacity
            };

            return View(dto);
        }

        // POST: Venues/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateVenueDTO dto)
        {
            if (id != dto.VenueId) return NotFound();

            var validator = new UpdateVenueValidator();
            var result = validator.Validate(dto);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.ErrorMessage);

                return View(dto);
            }

            try
            {
                await _mediator.Send(new UpdateVenueCommand(dto));
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Update failed.");
                return View(dto);
            }
        }

        // GET: Venues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var venues = await _mediator.Send(new GetAllVenuesQuery());
            var venue = venues.FirstOrDefault(v => v.VenueId == id);
            if (venue == null) return NotFound();

            return View(venue);
        }

        // POST: Venues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _mediator.Send(new DeleteVenueCommand(id));
            return RedirectToAction(nameof(Index));
        }
    }
}
