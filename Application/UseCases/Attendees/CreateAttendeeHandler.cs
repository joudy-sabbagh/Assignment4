using Core.Entities;
using Core.Interfaces;
using Application.DTOs;

namespace Application.UseCases.Attendees
{
    public class CreateAttendeeHandler
    {
        private readonly IAttendeeRepository _attendeeRepo;

        public CreateAttendeeHandler(IAttendeeRepository attendeeRepo)
        {
            _attendeeRepo = attendeeRepo;
        }

        public async Task Handle(CreateAttendeeDTO dto)
        {
            var attendee = new Attendee
            {
                Name = dto.Name,
                Email = dto.Email
            };

            await _attendeeRepo.AddAsync(attendee);
        }
    }
}
