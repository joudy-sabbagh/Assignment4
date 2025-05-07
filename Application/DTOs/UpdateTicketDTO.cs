// Application/DTOs/UpdateTicketDTO.cs
namespace Application.DTOs
{
    public class UpdateTicketDTO
    {
        public int Id { get; set; }                 
        public int EventId { get; set; }
        public int AttendeeId { get; set; }
        public string TicketType { get; set; } = string.Empty;
    }
}
