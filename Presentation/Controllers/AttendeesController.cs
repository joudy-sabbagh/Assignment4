using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Application.DTOs;
using Application.UseCases.Attendees;
using Application.Validators;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Presentation.Models;

namespace Presentation.Controllers
{
    public class AttendeesController : Controller
    {
        private const string CacheKey = "AllAttendees";

        private readonly IMediator _mediator;
        private readonly ILogger<AttendeesController> _logger;
        private readonly IMemoryCache _cache;

        public AttendeesController(
            IMediator mediator,
            ILogger<AttendeesController> logger,
            IMemoryCache cache)
        {
            _mediator = mediator;
            _logger = logger;
            _cache = cache;
        }

        // GET: Attendees
        public async Task<IActionResult> Index(int page = 1)
        {
            _logger.LogInformation("Fetching all attendees (with cache)");

            if (!_cache.TryGetValue(CacheKey, out List<AttendeeListDTO> all))
            {
                var result = await _mediator.Send(new GetAllAttendeesQuery());
                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to fetch attendees: {Error}", result.Error);
                    ModelState.AddModelError(string.Empty, result.Error ?? "Unknown error");
                    return View(new PagedListViewModel<AttendeeListDTO>());
                }
                all = result.Value!.ToList();
                _cache.Set(CacheKey, all, TimeSpan.FromMinutes(5));
            }

            const int PageSize = 20;
            var totalCount = all.Count;
            var items = all
                .Skip((page - 1) * PageSize)
                .Take(PageSize);

            var vm = new PagedListViewModel<AttendeeListDTO>
            {
                Items = items,
                PageNumber = page,
                PageSize = PageSize,
                TotalCount = totalCount
            };

            return View(vm);
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

            // invalidate cache
            _cache.Remove(CacheKey);

            return RedirectToAction(nameof(Index));
        }

        // GET: Attendees/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Rendering Edit form for Attendee {Id}", id);

            if (!_cache.TryGetValue(CacheKey, out List<AttendeeListDTO> all))
            {
                var result = await _mediator.Send(new GetAllAttendeesQuery());
                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to fetch for edit: {Error}", result.Error);
                    return RedirectToAction(nameof(Index));
                }
                all = result.Value!.ToList();
                _cache.Set(CacheKey, all, TimeSpan.FromMinutes(5));
            }

            var item = all.FirstOrDefault(a => a.Id == id);
            if (item == null)
            {
                _logger.LogWarning("Attendee {Id} not found", id);
                return NotFound();
            }

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

            // invalidate cache
            _cache.Remove(CacheKey);

            return RedirectToAction(nameof(Index));
        }

        // GET: Attendees/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Rendering Delete confirmation for Attendee {Id}", id);

            if (!_cache.TryGetValue(CacheKey, out List<AttendeeListDTO> all))
            {
                var result = await _mediator.Send(new GetAllAttendeesQuery());
                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to fetch for deletion: {Error}", result.Error);
                    return RedirectToAction(nameof(Index));
                }
                all = result.Value!.ToList();
                _cache.Set(CacheKey, all, TimeSpan.FromMinutes(5));
            }

            var item = all.FirstOrDefault(a => a.Id == id);
            if (item == null) return NotFound();

            return View(item);
        }

        // POST: Attendees/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("Deleting Attendee {Id}", id);
            await _mediator.Send(new DeleteAttendeeCommand(id));

            // invalidate cache
            _cache.Remove(CacheKey);

            return RedirectToAction(nameof(Index));
        }
    }
}
