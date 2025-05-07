namespace Application.DTOs
{
    public class TicketListDTO
    {
        public int Id { get; init; }
        public decimal Price { get; init; }
        public string Category { get; init; } = "";
        public int EventId { get; init; }        // NEW
        public string EventName { get; init; } = "";
        public int AttendeeId { get; init; }        // NEW
        public string AttendeeName { get; init; } = "";
    }
}
