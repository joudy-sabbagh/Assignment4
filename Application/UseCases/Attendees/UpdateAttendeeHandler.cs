using Core.Interfaces;
using Application.DTOs;

namespace Application.UseCases.Attendees
{
    public class UpdateAttendeeHandler
    {
        private readonly IAttendeeRepository _attendeeRepo;

        public UpdateAttendeeHandler(IAttendeeRepository attendeeRepo)
        {
            _attendeeRepo = attendeeRepo;
        }

        public async Task Handle(UpdateAttendeeDTO dto)
        {
            var attendee = await _attendeeRepo.GetByIdAsync(dto.AttendeeId);
            attendee.Name = dto.Name;
            attendee.Email = dto.Email;

            await _attendeeRepo.UpdateAsync(attendee);
        }
    }
}
