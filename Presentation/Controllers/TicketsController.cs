// Presentation/Controllers/TicketsController.cs
using System;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Application.UseCases.Tickets;
using Application.Validators;
using Domain.Entities;             // for TicketCategory
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class TicketsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IEventRepository _eventRepo;
        private readonly IAttendeeRepository _attendeeRepo;

        public TicketsController(
            IMediator mediator,
            IEventRepository eventRepo,
            IAttendeeRepository attendeeRepo)
        {
            _mediator = mediator;
            _eventRepo = eventRepo;
            _attendeeRepo = attendeeRepo;
        }

        // GET: Tickets
        public async Task<IActionResult> Index(
            string sortOrder,
            int? eventFilter,
            string categoryFilter)
        {
            ViewData["Events"] = await _eventRepo.GetAllAsync();
            ViewData["CategoryList"] = Enum.GetValues(typeof(TicketCategory))
                                        .Cast<TicketCategory>();

            var list = await _mediator.Send(
                new GetAllTicketsQuery(sortOrder, eventFilter, categoryFilter)
            );
            return View(list);
        }

        // GET: Tickets/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Events"] = await _eventRepo.GetAllAsync();
            ViewData["Attendees"] = await _attendeeRepo.GetAllAsync();
            ViewData["CategoryList"] = Enum.GetValues(typeof(TicketCategory))
                                        .Cast<TicketCategory>();
            return View();
        }

        // POST: Tickets/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTicketDTO dto)
        {
            var result = new CreateTicketValidator().Validate(dto);
            if (!result.IsValid)
            {
                foreach (var err in result.Errors)
                    ModelState.AddModelError(string.Empty, err.ErrorMessage);

                ViewData["Events"] = await _eventRepo.GetAllAsync();
                ViewData["Attendees"] = await _attendeeRepo.GetAllAsync();
                ViewData["CategoryList"] = Enum.GetValues(typeof(TicketCategory))
                                            .Cast<TicketCategory>();
                return View(dto);
            }
            await _mediator.Send(new CreateTicketCommand(dto));
            return RedirectToAction(nameof(Index));
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var list = await _mediator.Send(
                new GetAllTicketsQuery("", null, null)
            );
            var item = list.FirstOrDefault(t => t.Id == id);
            if (item == null) return NotFound();

            ViewData["Events"] = await _eventRepo.GetAllAsync();
            ViewData["Attendees"] = await _attendeeRepo.GetAllAsync();
            ViewData["CategoryList"] = Enum.GetValues(typeof(TicketCategory))
                                        .Cast<TicketCategory>();

            return View(new UpdateTicketDTO
            {
                Id = item.Id,
                EventId = item.EventId,
                AttendeeId = item.AttendeeId,
                TicketType = item.Category
            });
        }

        // POST: Tickets/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateTicketDTO dto)
        {
            if (id != dto.Id) return NotFound();

            var result = new UpdateTicketValidator().Validate(dto);
            if (!result.IsValid)
            {
                foreach (var err in result.Errors)
                    ModelState.AddModelError(string.Empty, err.ErrorMessage);

                ViewData["Events"] = await _eventRepo.GetAllAsync();
                ViewData["Attendees"] = await _attendeeRepo.GetAllAsync();
                ViewData["CategoryList"] = Enum.GetValues(typeof(TicketCategory))
                                            .Cast<TicketCategory>();
                return View(dto);
            }
            await _mediator.Send(new UpdateTicketCommand(dto));
            return RedirectToAction(nameof(Index));
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var list = await _mediator.Send(
                new GetAllTicketsQuery("", null, null)
            );
            var item = list.FirstOrDefault(t => t.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _mediator.Send(new DeleteTicketCommand(id));
            return RedirectToAction(nameof(Index));
        }
    }
}
