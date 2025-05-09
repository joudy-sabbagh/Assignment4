namespace Application.DTOs
{
    public class VenueListDTO
    {
        public int Id { get; init; }
        public string Name { get; init; } = "";
        public string Location { get; init; } = "";
        public int Capacity { get; init; }
    }
}
