namespace Application.DTOs
{
    public class UpdateEventDTO
    {
        public int EventId { get; set; }
        public string Name { get; set; } = string.Empty;

        public DateTime EventDate { get; set; }

        public decimal NormalPrice { get; set; }

        public decimal VIPPrice { get; set; }

        public decimal BackstagePrice { get; set; }
        public int VenueId { get; set; }
    }
}
