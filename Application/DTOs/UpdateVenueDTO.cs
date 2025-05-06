namespace Application.DTOs;

public class UpdateVenueDTO
{
    public int VenueId { get; set; }
    public string Name { get; set; } = null!;
    public int Capacity { get; set; }
}
