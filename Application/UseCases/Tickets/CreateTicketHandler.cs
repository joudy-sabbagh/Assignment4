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

        public CreateTicketHandler(ITicketRepository ticketRepo, IEventRepository eventRepo)
        {
            _ticketRepo = ticketRepo;
            _eventRepo = eventRepo;
        }

        public async Task<int> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var ev = await _eventRepo.GetByIdAsync(dto.EventId);
            if (ev == null) throw new Exception("Event not found");

            var ticket = new Ticket
            {
                EventId = dto.EventId,
                AttendeeId = dto.AttendeeId,
                TicketType = dto.TicketType,
                Price = dto.TicketType switch
                {
                    "Normal" => ev.NormalPrice,
                    "VIP" => ev.VIPPrice,
                    "Backstage" => ev.BackstagePrice,
                    _ => ev.NormalPrice
                }
            };

            await _ticketRepo.AddAsync(ticket);
            return ticket.Id;
        }
    }
}
