using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMVCApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MyMVCApp.Controllers
{
    public class EventsController : Controller
    {
        private readonly IEventRepository _eventRepo;

        public EventsController(IEventRepository eventRepo)
        {
            _eventRepo = eventRepo;
        }

        // GET: Events with search functionality
        public async Task<IActionResult> Index(string searchString)
        {
            var eventsQuery = _context.Events.Include(e => e.Venue).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                // Case-insensitive search for event name.
                string lowerSearch = searchString.ToLower();
                eventsQuery = eventsQuery.Where(e => e.Name.ToLower().Contains(lowerSearch));
            }
            
            return View(await eventsQuery.ToListAsync());
        }
        
        // GET: Events/Create
        public IActionResult Create()
        {
            ViewData["Venues"] = _context.Venues.ToList();
            return View();
        }
        
        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event ev)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ev);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Venues"] = _context.Venues.ToList();
            return View(ev);
        }
        
        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            
            var ev = await _context.Events.FindAsync(id);
            if (ev == null)
                return NotFound();
            
            ViewData["Venues"] = _context.Venues.ToList();
            return View(ev);
        }
        
        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event ev)
        {
            if (id != ev.EventId)
                return NotFound();
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ev);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Events.Any(e => e.EventId == ev.EventId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Venues"] = _context.Venues.ToList();
            return View(ev);
        }
        
        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
            
            var ev = await _context.Events.Include(e => e.Venue)
                                          .FirstOrDefaultAsync(e => e.EventId == id);
            if (ev == null)
                return NotFound();
            return View(ev);
        }
        
        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev == null)
                return NotFound();
            _context.Events.Remove(ev);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
