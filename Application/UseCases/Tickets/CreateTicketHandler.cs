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

        var ev = await _eventRepo.GetByIdAsync(dto.EventId);
        if (ev == null)
            throw new Exception("Event not found");

        ticket.Price = ticket.TicketType switch
        {
            "Normal" => ev.NormalPrice,
            "VIP" => ev.VIPPrice,
            "Backstage" => ev.BackstagePrice,
            _ => ev.NormalPrice
        };

        await _ticketRepo.AddAsync(ticket);
    }
}
