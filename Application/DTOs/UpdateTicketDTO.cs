namespace Application.DTOs
{
    public class UpdateTicketDTO
    {
        public required int Id { get; init; }
        public required int EventId { get; init; }
        public required int AttendeeId { get; init; }
        public required string TicketType { get; init; }
    }
}
