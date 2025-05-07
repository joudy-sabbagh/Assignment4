namespace Application.DTOs
{
    public class EventListDTO
    {
        public int Id { get; init; }
        public string Name { get; init; } = "";
        public DateTime EventDate { get; init; }
        public decimal NormalPrice { get; init; }
        public decimal VIPPrice { get; init; }
        public decimal BackstagePrice { get; init; }
        public int VenueId { get; init; }        // NEW
        public string VenueName { get; init; } = "";
    }
}
