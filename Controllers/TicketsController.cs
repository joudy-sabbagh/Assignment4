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
        private readonly ITicketRepository _ticketRepo;
        private readonly CreateTicketHandler _createTicketHandler;
        private readonly UpdateTicketHandler _updateTicketHandler;
        private readonly DeleteTicketHandler _deleteTicketHandler;
        private readonly GetAllTicketsHandler _getAllTicketsHandler;

        public TicketsController(
            ITicketRepository ticketRepo,
            IEventRepository eventRepo,
            IAttendeeRepository attendeeRepo,
            CreateTicketHandler createTicketHandler,
            UpdateTicketHandler updateTicketHandler,
            DeleteTicketHandler deleteTicketHandler,
            GetAllTicketsHandler getAllTicketsHandler)
        {
            _ticketRepo = ticketRepo;
            _eventRepo = eventRepo;
            _attendeeRepo = attendeeRepo;
            _createTicketHandler = createTicketHandler;
            _updateTicketHandler = updateTicketHandler;
            _deleteTicketHandler = deleteTicketHandler;
            _getAllTicketsHandler = getAllTicketsHandler;
        }



        // GET: Tickets with filtering and sorting.
        public async Task<IActionResult> Index(string sortOrder, int? eventFilter, string categoryFilter)
        {
            ViewData["PriceSortParam"] = sortOrder == "price_asc" ? "price_desc" : "price_asc";
            ViewData["Events"] = await _eventRepo.GetAllAsync();  // Get events from the repository
            ViewData["CategoryList"] = Enum.GetValues(typeof(TicketCategory)).Cast<TicketCategory>();

            var tickets = await _getAllTicketsHandler.Handle(sortOrder, eventFilter, categoryFilter);

            return View(tickets);
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
        public async Task<IActionResult> Edit(int id, UpdateTicketDTO dto)
        {
            if (id != dto.TicketId)
                return NotFound();

            var validator = new UpdateTicketValidator();
            var result = validator.Validate(dto);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.ErrorMessage);

                return View(dto);
            }

            try
            {
                await _updateTicketHandler.Handle(dto);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Update failed.");
                return View(dto);
            }
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
            try
            {
                await _deleteTicketHandler.Handle(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Delete failed.");
                return RedirectToAction(nameof(Index)); 
            }
        }
    }
}
