namespace Application.DTOs;

public class CreateVenueDTO
{
    public string Name { get; set; } = null!;
    public int Capacity { get; set; }
    public string Location { get; set; } = string.Empty;
}
