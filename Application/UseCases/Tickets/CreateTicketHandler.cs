// Application/UseCases/Tickets/CreateTicketHandler.cs
using MediatR;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using Domain.ValueObjects;

namespace Application.UseCases.Tickets
{
    public class CreateTicketHandler : IRequestHandler<CreateTicketCommand, int>
    {
        private readonly ITicketRepository _ticketRepo;
        private readonly IEventRepository _eventRepo;
        private readonly IMediator _mediator;

        public CreateTicketHandler(
            ITicketRepository ticketRepo,
            IEventRepository eventRepo,
            IMediator mediator)
        {
            _ticketRepo = ticketRepo;
            _eventRepo = eventRepo;
            _mediator = mediator;
        }

        public async Task<int> Handle(CreateTicketCommand request, CancellationToken ct)
        {
            var dto = request.Dto;
            var ev = await _eventRepo.GetByIdAsync(dto.EventId)
                      ?? throw new KeyNotFoundException();

            var category = Enum.TryParse<TicketCategory>(dto.TicketType, true, out var c)
                           ? c
                           : TicketCategory.Normal;

            var ticket = new Ticket(
                dto.TicketType,
                new Money(dto.Price),
                category,
                dto.EventId,
                dto.AttendeeId
            );

            await _ticketRepo.AddAsync(ticket);
            await _mediator.Publish(new TicketPurchasedEvent(ticket.Id), ct);
            return ticket.Id;
        }
    }
}
