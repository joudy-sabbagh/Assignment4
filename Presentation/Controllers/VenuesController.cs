// Presentation/Controllers/VenuesController.cs
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Application.UseCases.Venues;
using Application.Validators;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class VenuesController : Controller
    {
        private readonly IMediator _mediator;

        public VenuesController(IMediator mediator) => _mediator = mediator;

        // GET: Venues
        public async Task<IActionResult> Index()
        {
            var list = await _mediator.Send(new GetAllVenuesQuery());
            return View(list);
        }

        // GET: Venues/Create
        public IActionResult Create() => View();

        // POST: Venues/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateVenueDTO dto)
        {
            var result = new CreateVenueValidator().Validate(dto);
            if (!result.IsValid)
            {
                foreach (var err in result.Errors)
                    ModelState.AddModelError(string.Empty, err.ErrorMessage);
                return View(dto);
            }
            await _mediator.Send(new CreateVenueCommand(dto));
            return RedirectToAction(nameof(Index));
        }

        // GET: Venues/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var list = await _mediator.Send(new GetAllVenuesQuery());
            var item = list.FirstOrDefault(v => v.Id == id);
            if (item == null) return NotFound();

            return View(new UpdateVenueDTO
            {
                Id = item.Id,
                Name = item.Name,
                Location = item.Location,
                Capacity = item.Capacity
            });
        }

        // POST: Venues/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateVenueDTO dto)
        {
            if (id != dto.Id) return NotFound();

            var result = new UpdateVenueValidator().Validate(dto);
            if (!result.IsValid)
            {
                foreach (var err in result.Errors)
                    ModelState.AddModelError(string.Empty, err.ErrorMessage);
                return View(dto);
            }
            await _mediator.Send(new UpdateVenueCommand(dto));
            return RedirectToAction(nameof(Index));
        }

        // GET: Venues/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var list = await _mediator.Send(new GetAllVenuesQuery());
            var item = list.FirstOrDefault(v => v.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        // POST: Venues/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _mediator.Send(new DeleteVenueCommand(id));
            return RedirectToAction(nameof(Index));
        }
    }
}
