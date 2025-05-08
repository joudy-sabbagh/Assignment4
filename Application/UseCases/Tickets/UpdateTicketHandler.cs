// Application/UseCases/Tickets/UpdateTicketHandler.cs
using MediatR;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;

namespace Application.UseCases.Tickets
{
    public class UpdateTicketHandler : IRequestHandler<UpdateTicketCommand, Unit>
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

        public async Task<Unit> Handle(UpdateTicketCommand request, CancellationToken ct)
        {
            var dto = request.Dto;
            var ticket = await _ticketRepo.GetByIdAsync(dto.Id)
                         ?? throw new KeyNotFoundException();
            var ev = await _eventRepo.GetByIdAsync(dto.EventId)
                     ?? throw new KeyNotFoundException();

            var category = Enum.TryParse<TicketCategory>(dto.TicketType, true, out var c)
                           ? c
                           : TicketCategory.Normal;

            ticket.UpdateTypeAndPrice(
                dto.TicketType,
                new Money(dto.Price),
                category,
                dto.EventId,
                dto.AttendeeId
            );

            await _ticketRepo.UpdateAsync(ticket);
            return Unit.Value;
        }
    }
}
