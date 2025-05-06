namespace Application.DTOs;

public class UpdateTicketDTO
{
    public int TicketId { get; set; }
    public int EventId { get; set; }
    public int AttendeeId { get; set; }
    public string TicketType { get; set; } = null!;
}
