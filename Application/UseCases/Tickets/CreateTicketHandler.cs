// Application/UseCases/Tickets/CreateTicketHandler.cs
using System;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Tickets
{
    public class CreateTicketHandler : IRequestHandler<CreateTicketCommand, int>
    {
        private readonly ITicketRepository _ticketRepo;
        private readonly IEventRepository _eventRepo;

        public CreateTicketHandler(
            ITicketRepository ticketRepo,
            IEventRepository eventRepo)
        {
            _ticketRepo = ticketRepo;
            _eventRepo = eventRepo;
        }

        public async Task<int> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            // 1. Ensure the event exists
            var ev = await _eventRepo.GetByIdAsync(dto.EventId);
            if (ev == null)
                throw new KeyNotFoundException($"Event with ID {dto.EventId} not found.");

            // 2. Determine price based on type
            var price = dto.TicketType switch
            {
                "Normal" => ev.NormalPrice,
                "VIP" => ev.VIPPrice,
                "Backstage" => ev.BackstagePrice,
                _ => ev.NormalPrice
            };

            // 3. Parse the enum category (fallback to Normal if invalid)
            var category = Enum.TryParse<TicketCategory>(dto.TicketType, true, out var cat)
                ? cat
                : TicketCategory.Normal;

            // 4. Use the domain constructor (with guard clauses)
            var ticket = new Ticket(
                dto.TicketType,
                price,
                category,
                dto.EventId,
                dto.AttendeeId
            );

            // 5. Persist and return the new Id
            await _ticketRepo.AddAsync(ticket);
            return ticket.Id;
        }
    }
}
