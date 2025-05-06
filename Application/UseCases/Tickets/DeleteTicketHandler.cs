namespace Application.UseCases.Tickets;

public class DeleteTicketHandler
{
    private readonly ITicketRepository _ticketRepo;

    public DeleteTicketHandler(ITicketRepository ticketRepo)
    {
        _ticketRepo = ticketRepo;
    }

    public async Task Handle(int ticketId)
    {
        var ticket = await _ticketRepo.GetByIdAsync(ticketId);
        if (ticket == null)
            throw new Exception("Ticket not found");

        await _ticketRepo.DeleteAsync(ticketId);
    }
}
