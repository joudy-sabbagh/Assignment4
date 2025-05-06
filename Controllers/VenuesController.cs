using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMVCApp.Models;
using System.Threading.Tasks;

namespace MyMVCApp.Controllers
{
    public class VenuesController : Controller
    {
        private readonly IEventRepository _eventRepo;

        public EventsController(IEventRepository eventRepo)
        {
            _eventRepo = eventRepo;
        }

        // GET: Venues
        public async Task<IActionResult> Index() =>
            View(await _context.Venues.ToListAsync());

        // GET: Venues/Create
        public IActionResult Create() => View();

        // POST: Venues/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VenueId,Name,Location")] Venue venue)
        {
            if (ModelState.IsValid)
            {
                _context.Add(venue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }

        // GET: Venues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            var venue = await _context.Venues.FindAsync(id);
            if (venue == null)
                return NotFound();
            return View(venue);
        }

        // POST: Venues/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VenueId,Name,Location")] Venue venue)
        {
            if (id != venue.VenueId)
                return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(venue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
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
            var venue = await _context.Venues.FindAsync(id);
            if (venue == null)
                return NotFound();
            _context.Venues.Remove(venue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
