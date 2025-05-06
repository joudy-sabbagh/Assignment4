using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMVCApp.Models;
using System.Threading.Tasks;

namespace MyMVCApp.Controllers
{
    public class VenuesController : Controller
    {
        private readonly IVenueRepository _venueRepo;
        private readonly CreateVenueHandler _createVenueHandler;
        private readonly GetAllVenuesHandler _getAllVenuesHandler;
        private readonly EditVenueHandler _editVenueHandler;
        private readonly DeleteVenueHandler _deleteVenueHandler;


        public VenuesController(
            IVenueRepository venueRepo,
            CreateVenueHandler createVenueHandler,
            GetAllVenuesHandler getAllVenuesHandler,
            EditVenueHandler editVenueHandler,
            DeleteVenueHandler deleteVenueHandler)
        {
            _venueRepo = venueRepo;
            _createVenueHandler = createVenueHandler;
            _getAllVenuesHandler = getAllVenuesHandler;
            _editVenueHandler = editVenueHandler;
            _deleteVenueHandler = deleteVenueHandler;
        }

        // GET: Venues
        public async Task<IActionResult> Index()
        {
            var venues = await _getAllVenuesHandler.Handle();
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

            await _createVenueHandler.Handle(dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: Venues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            var venue = await _venueRepo.GetByIdAsync(id);
            if (venue == null)
                return NotFound();
            return View(venue);
        }

        // POST: Venues/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateVenueDTO dto)
        {
            if (id != dto.VenueId)
                return NotFound();

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
                await _editVenueHandler.Handle(dto);
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
            if (id == null)
                return NotFound();
            var venue = await _context.Venues.FirstOrDefaultAsync(v => v.VenueId == id);
            if (venue == null)
                return NotFound();
            return View(venue);
        }

        // POST: Venues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _deleteVenueHandler.Handle(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
