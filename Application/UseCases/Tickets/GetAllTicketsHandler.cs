namespace Application.UseCases.Tickets;

public class GetAllTicketsHandler
{
    private readonly ITicketRepository _ticketRepo;

    public GetAllTicketsHandler(ITicketRepository ticketRepo)
    {
        _ticketRepo = ticketRepo;
    }

    public async Task<IEnumerable<Ticket>> Handle(string sortOrder, int? eventFilter, string categoryFilter)
    {
        var ticketsQuery = _ticketRepo.GetAllWithEventAndAttendee();

        // Apply event filter
        if (eventFilter.HasValue)
        {
            ticketsQuery = ticketsQuery.Where(t => t.EventId == eventFilter.Value);
        }

        // Parse category filter
        if (!string.IsNullOrEmpty(categoryFilter) && Enum.TryParse<TicketCategory>(categoryFilter, out var parsedCategory))
        {
            ticketsQuery = ticketsQuery.Where(t => t.Category == parsedCategory);
        }

        // Apply sorting by price
        switch (sortOrder)
        {
            case "price_desc":
                ticketsQuery = ticketsQuery.OrderByDescending(t => (double)t.Price);
                break;
            default:
                ticketsQuery = ticketsQuery.OrderBy(t => (double)t.Price);
                break;
        }

        return await ticketsQuery.ToListAsync();
    }
}
