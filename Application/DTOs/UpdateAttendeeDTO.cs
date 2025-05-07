// Application/DTOs/UpdateAttendeeDTO.cs
namespace Application.DTOs
{
    public class UpdateAttendeeDTO
    {
        public required int Id { get; init; }       
        public required string Name { get; init; }
        public required string Email { get; init; }
    }
}
