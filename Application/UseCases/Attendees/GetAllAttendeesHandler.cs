using Core.Entities;
using Core.Interfaces;

namespace Application.UseCases.Attendees
{
    public class GetAllAttendeesHandler
    {
        private readonly IAttendeeRepository _attendeeRepo;

        public GetAllAttendeesHandler(IAttendeeRepository attendeeRepo)
        {
            _attendeeRepo = attendeeRepo;
        }

        public async Task<List<Attendee>> Handle()
        {
            return await _attendeeRepo.GetAllAsync();
        }
    }
}
