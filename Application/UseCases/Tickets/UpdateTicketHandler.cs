// Application/UseCases/Tickets/UpdateTicketHandler.cs
using System;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Tickets
{
    public class UpdateTicketHandler : IRequestHandler<UpdateTicketCommand>
    {
        private readonly ITicketRepository _ticketRepo;
        private readonly IEventRepository _eventRepo;

        public UpdateTicketHandler(
            ITicketRepository ticketRepo,
            IEventRepository eventRepo)
        {
            _ticketRepo = ticketRepo;
            _eventRepo = eventRepo;
        }

        public async Task Handle(UpdateTicketCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            var ticket = await _ticketRepo.GetByIdAsync(dto.Id);
            if (ticket == null)
                throw new KeyNotFoundException($"Ticket with ID {dto.Id} not found.");

            var ev = await _eventRepo.GetByIdAsync(dto.EventId);
            if (ev == null)
                throw new KeyNotFoundException($"Event with ID {dto.EventId} not found.");

            // recalc price and category
            var price = dto.TicketType switch
            {
                "VIP" => ev.VIPPrice,
                "Backstage" => ev.BackstagePrice,
                _ => ev.NormalPrice
            };
            var category = Enum.TryParse<TicketCategory>(
                dto.TicketType, true, out var cat)
                ? cat
                : TicketCategory.Normal;

            // use domain helper
            ticket.UpdateTypeAndPrice(
                dto.TicketType,
                price,
                category,
                dto.EventId,
                dto.AttendeeId
            );

            await _ticketRepo.UpdateAsync(ticket);
        }
    }
}
