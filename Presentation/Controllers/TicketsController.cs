using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Validators;
using Application.UseCases.Tickets;
using Domain.Interfaces;
using Domain.Entities;
using MediatR;

namespace Presentation.Controllers
{
    public class TicketsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IEventRepository _eventRepo;
        private readonly IAttendeeRepository _attendeeRepo;

        public TicketsController(IMediator mediator, IEventRepository eventRepo, IAttendeeRepository attendeeRepo)
        {
            _mediator = mediator;
            _eventRepo = eventRepo;
            _attendeeRepo = attendeeRepo;
        }

        // GET: Tickets with filtering and sorting
        public async Task<IActionResult> Index(string sortOrder, int? eventFilter, string categoryFilter)
        {
            ViewData["PriceSortParam"] = sortOrder == "price_asc" ? "price_desc" : "price_asc";
            ViewData["Events"] = await _eventRepo.GetAllAsync();
            ViewData["CategoryList"] = Enum.GetValues(typeof(TicketCategory)).Cast<TicketCategory>();

            var tickets = await _mediator.Send(new GetAllTicketsQuery(sortOrder, eventFilter, categoryFilter));
            return View(tickets);
        }

        // GET: Tickets/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Events"] = await _eventRepo.GetAllAsync();
            ViewData["Attendees"] = await _attendeeRepo.GetAllAsync();
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

            await _mediator.Send(new CreateTicketCommand(dto));
            return RedirectToAction(nameof(Index));
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var ticket = await _mediator.Send(new GetAllTicketsQuery("", null, null));
            var existing = ticket.FirstOrDefault(t => t.TicketId == id);
            if (existing == null) return NotFound();

            var dto = new UpdateTicketDTO
            {
                TicketId = existing.TicketId,
                EventId = existing.EventId,
                AttendeeId = existing.AttendeeId,
                TicketType = existing.TicketType
            };

            ViewData["Events"] = await _eventRepo.GetAllAsync();
            ViewData["Attendees"] = await _attendeeRepo.GetAllAsync();
            ViewData["CategoryList"] = Enum.GetValues(typeof(TicketCategory)).Cast<TicketCategory>();
            return View(dto);
        }

        // POST: Tickets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateTicketDTO dto)
        {
            if (id != dto.TicketId) return NotFound();

            var validator = new UpdateTicketValidator();
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

            try
            {
                await _mediator.Send(new UpdateTicketCommand(dto));
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

            var ticketList = await _mediator.Send(new GetAllTicketsQuery("", null, null));
            var ticket = ticketList.FirstOrDefault(t => t.TicketId == id);

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
                await _mediator.Send(new DeleteTicketCommand(id));
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
