namespace Application.DTOs;

public class CreateTicketDTO
{
    public int EventId { get; set; }
    public int AttendeeId { get; set; }
    public string TicketType { get; set; } = null!;
}