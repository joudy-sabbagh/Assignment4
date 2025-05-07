namespace Application.DTOs
{
    public class UpdateAttendeeDTO
    {
        public required int AttendeeId { get; init; }
        public required string Name { get; init; }
        public required string Email { get; init; }
    }
}