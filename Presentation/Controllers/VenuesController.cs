using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Application.DTOs;
using Application.UseCases.Venues;
using Application.Validators;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Presentation.Models;

namespace Presentation.Controllers
{
    // Require any authenticated user
    [Authorize]
    public class VenuesController : Controller
    {
        private const string CacheKey = "AllVenues";
        private readonly IMediator _mediator;
        private readonly ILogger<VenuesController> _logger;
        private readonly IMemoryCache _cache;

        public VenuesController(
            IMediator mediator,
            ILogger<VenuesController> logger,
            IMemoryCache cache)
        {
            _mediator = mediator;
            _logger = logger;
            _cache = cache;
        }

        // GET: Venues
        public async Task<IActionResult> Index(int page = 1)
        {
            _logger.LogInformation("Fetching all venues (with cache)");

            if (!_cache.TryGetValue(CacheKey, out List<VenueListDTO> all))
            {
                var result = await _mediator.Send(new GetAllVenuesQuery());
                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to fetch venues: {Error}", result.Error);
                    ModelState.AddModelError(string.Empty, result.Error ?? "Unknown error");
                    return View(new PagedListViewModel<VenueListDTO>());
                }

                all = result.Value!.ToList();
                _cache.Set(CacheKey, all, TimeSpan.FromMinutes(5));
            }

            const int PageSize = 20;
            var totalCount = all.Count;
            var items = all
                        .Skip((page - 1) * PageSize)
                        .Take(PageSize);

            var vm = new PagedListViewModel<VenueListDTO>
            {
                Items = items,
                PageNumber = page,
                PageSize = PageSize,
                TotalCount = totalCount
            };

            return View(vm);
        }

        // Only Admins may create venues
        [Authorize(Roles = "Admin")]
        // GET: Venues/Create
        public IActionResult Create()
        {
            _logger.LogInformation("Rendering Create Venue form");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        // POST: Venues/Create
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

            _cache.Remove(CacheKey);
            return RedirectToAction(nameof(Index));
        }

        // Only Admins may edit venues
        [Authorize(Roles = "Admin")]
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

            var item = result.Value!.FirstOrDefault(v => v.Id == id);
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

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        // POST: Venues/Edit/5
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

            _cache.Remove(CacheKey);
            return RedirectToAction(nameof(Index));
        }

        // Only Admins may delete venues
        [Authorize(Roles = "Admin")]
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

            var item = result.Value!.FirstOrDefault(v => v.Id == id);
            if (item == null)
            {
                _logger.LogWarning("Venue {Id} not found for deletion", id);
                return NotFound();
            }

            return View(item);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        // POST: Venues/Delete/5
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("Deleting venue {Id}", id);

            await _mediator.Send(new DeleteVenueCommand(id));
            _logger.LogInformation("Venue {Id} deleted", id);

            _cache.Remove(CacheKey);
            return RedirectToAction(nameof(Index));
        }
    }
}
