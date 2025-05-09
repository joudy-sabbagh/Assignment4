namespace Application.DTOs
{
    public class CreateTicketDTO
    {
        public required int EventId { get; init; }
        public required int AttendeeId { get; init; }
        public required string TicketType { get; init; }
    }
}