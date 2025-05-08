namespace Domain.Services
{
    public interface ITicketPricingService
    {
        decimal GetPrice(Domain.Entities.Event ev, string ticketType);
    }
}
