namespace Application.DTOs;

public class UpdateEventDTO
{
    public int EventId { get; set; }
    public string Name { get; set; } = null!;
    public DateTime Date { get; set; }
    public decimal NormalPrice { get; set; }
    public decimal VipPrice { get; set; }
    public decimal BackstagePrice { get; set; }
    public int VenueId { get; set; }
}
