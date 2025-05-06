using Application.DTOs;
using Core.Entities;
using Core.Interfaces;

namespace Application.UseCases.Tickets;

public class CreateTicketHandler
{
    private readonly ITicketRepository _ticketRepo;
    private readonly IEventRepository _eventRepo;
    private readonly IAttendeeRepository _attendeeRepo;

    public CreateTicketHandler(
        ITicketRepository ticketRepo,
        IEventRepository eventRepo,
        IAttendeeRepository attendeeRepo)
    {
        _ticketRepo = ticketRepo;
        _eventRepo = eventRepo;
        _attendeeRepo = attendeeRepo;
    }

    public async Task Handle(CreateTicketDTO dto)
    {
        var ticket = new Ticket
        {
            EventId = dto.EventId,
            AttendeeId = dto.AttendeeId,
            TicketType = dto.TicketType
        };

        await _ticketRepo.AddAsync(ticket);
    }
}
