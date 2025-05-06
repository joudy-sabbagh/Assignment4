using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMVCApp.Models;
using System.Threading.Tasks;

namespace MyMVCApp.Controllers
{
    public class AttendeesController : Controller
    {
        private readonly IEventRepository _eventRepo;

        public EventsController(IEventRepository eventRepo)
        {
            _eventRepo = eventRepo;
        }

        // GET: Attendees
        public async Task<IActionResult> Index() =>
            View(await _context.Attendees.ToListAsync());

        // GET: Attendees/Create
        public IActionResult Create() => View();

        // POST: Attendees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AttendeeId,Name,Email")] Attendee attendee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attendee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(attendee);
        }

        // GET: Attendees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            var attendee = await _context.Attendees.FindAsync(id);
            if (attendee == null)
                return NotFound();
            return View(attendee);
        }

        // POST: Attendees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AttendeeId,Name,Email")] Attendee attendee)
        {
            if (id != attendee.AttendeeId)
                return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(attendee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(attendee);
        }

        // GET: Attendees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
            var attendee = await _context.Attendees.FirstOrDefaultAsync(a => a.AttendeeId == id);
            if (attendee == null)
                return NotFound();
            return View(attendee);
        }

        // POST: Attendees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attendee = await _context.Attendees.FindAsync(id);
            if (attendee == null)
                return NotFound();
            _context.Attendees.Remove(attendee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
