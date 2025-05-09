namespace Application.DTOs
{
    public class UpdateVenueDTO
    {
        public int Id { get; set; }                
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string Location { get; set; } = string.Empty;
    }
}
