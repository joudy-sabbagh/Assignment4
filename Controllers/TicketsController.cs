using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMVCApp.Controllers
{
    public class TicketsController : Controller
    {
        private readonly CreateTicketHandler _createTicketHandler;

        public TicketsController(
            ITicketRepository ticketRepo,
            IEventRepository eventRepo,
            IAttendeeRepository attendeeRepo,
            CreateTicketHandler createTicketHandler)
        {
            _ticketRepo = ticketRepo;
            _eventRepo = eventRepo;
            _attendeeRepo = attendeeRepo;
            _createTicketHandler = createTicketHandler;
        }


        // GET: Tickets with filtering and sorting.
        public async Task<IActionResult> Index(string sortOrder, int? eventFilter, string categoryFilter)
        {
            ViewData["PriceSortParam"] = sortOrder == "price_asc" ? "price_desc" : "price_asc";
            ViewData["Events"] = _context.Events.ToList();
            ViewData["CategoryList"] = Enum.GetValues(typeof(TicketCategory)).Cast<TicketCategory>();

            // Parse the category filter.
            TicketCategory? parsedCategory = null;
            if (!string.IsNullOrEmpty(categoryFilter) && Enum.TryParse<TicketCategory>(categoryFilter, out TicketCategory cat))
            {
                parsedCategory = cat;
            }

            var ticketsQuery = _context.Tickets
                .Include(t => t.Event)
                .Include(t => t.Attendee)
                .AsQueryable();

            if (eventFilter.HasValue)
            {
                ticketsQuery = ticketsQuery.Where(t => t.EventId == eventFilter.Value);
            }
            if (parsedCategory.HasValue)
            {
                ticketsQuery = ticketsQuery.Where(t => t.Category == parsedCategory.Value);
            }

            // Sort by price (casting to double for SQLite compatibility)
            switch (sortOrder)
            {
                case "price_desc":
                    ticketsQuery = ticketsQuery.OrderByDescending(t => (double)t.Price);
                    break;
                default:
                    ticketsQuery = ticketsQuery.OrderBy(t => (double)t.Price);
                    break;
            }

            return View(await ticketsQuery.ToListAsync());
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            ViewData["Events"] = _context.Events.ToList();
            ViewData["Attendees"] = _context.Attendees.ToList();
            ViewData["CategoryList"] = Enum.GetValues(typeof(TicketCategory)).Cast<TicketCategory>();
            return View();
        }

        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTicketDTO dto)
        {
            var validator = new CreateTicketValidator();
            var result = validator.Validate(dto);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.ErrorMessage);

                ViewData["Events"] = await _eventRepo.GetAllAsync();
                ViewData["Attendees"] = await _attendeeRepo.GetAllAsync();
                ViewData["CategoryList"] = Enum.GetValues(typeof(TicketCategory)).Cast<TicketCategory>();
                return View(dto);
            }

            await _createTicketHandler.Handle(dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return NotFound();

            ViewData["Events"] = _context.Events.ToList();
            ViewData["Attendees"] = _context.Attendees.ToList();
            ViewData["CategoryList"] = Enum.GetValues(typeof(TicketCategory)).Cast<TicketCategory>();
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TicketId,EventId,AttendeeId,Category")] Ticket updatedTicket)
        {
            if (id != updatedTicket.TicketId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var ticket = await _context.Tickets.FindAsync(id);
                    if (ticket == null)
                        return NotFound();

                    ticket.EventId = updatedTicket.EventId;
                    ticket.AttendeeId = updatedTicket.AttendeeId;
                    ticket.Category = updatedTicket.Category;

                    // Look up the event to update the price.
                    var ev = await _context.Events.FindAsync(ticket.EventId);
                    if (ev != null)
                    {
                        ticket.Price = ticket.Category switch
                        {
                            TicketCategory.Normal    => ev.NormalPrice,
                            TicketCategory.VIP       => ev.VIPPrice,
                            TicketCategory.Backstage => ev.BackstagePrice,
                            _                        => ev.NormalPrice
                        };
                    }

                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Tickets.Any(e => e.TicketId == updatedTicket.TicketId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Events"] = _context.Events.ToList();
            ViewData["Attendees"] = _context.Attendees.ToList();
            ViewData["CategoryList"] = Enum.GetValues(typeof(TicketCategory)).Cast<TicketCategory>();
            return View(updatedTicket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var ticket = await _context.Tickets
                .Include(t => t.Event)
                .Include(t => t.Attendee)
                .FirstOrDefaultAsync(t => t.TicketId == id);
            if (ticket == null) return NotFound();
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return NotFound();
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
