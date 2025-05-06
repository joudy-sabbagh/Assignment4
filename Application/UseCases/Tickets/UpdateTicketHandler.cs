using Application.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Tickets
{
    public class UpdateTicketHandler : IRequestHandler<UpdateTicketCommand>
    {
        private readonly ITicketRepository _ticketRepo;
        private readonly IEventRepository _eventRepo;

        public UpdateTicketHandler(ITicketRepository ticketRepo, IEventRepository eventRepo)
        {
            _ticketRepo = ticketRepo;
            _eventRepo = eventRepo;
        }

        public async Task Handle(UpdateTicketCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            var ticket = await _ticketRepo.GetByIdAsync(dto.TicketId);
            if (ticket == null) throw new Exception("Ticket not found");

            var ev = await _eventRepo.GetByIdAsync(dto.EventId);
            if (ev == null) throw new Exception("Event not found");

            ticket.EventId = dto.EventId;
            ticket.AttendeeId = dto.AttendeeId;
            ticket.TicketType = dto.TicketType;
            ticket.Price = dto.TicketType switch
            {
                "Normal" => ev.NormalPrice,
                "VIP" => ev.VIPPrice,
                "Backstage" => ev.BackstagePrice,
                _ => ev.NormalPrice
            };

            await _ticketRepo.UpdateAsync(ticket);
        }
    }
}
