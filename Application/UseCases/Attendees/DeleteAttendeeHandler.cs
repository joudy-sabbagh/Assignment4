using Core.Interfaces;

namespace Application.UseCases.Attendees
{
    public class DeleteAttendeeHandler
    {
        private readonly IAttendeeRepository _attendeeRepo;

        public DeleteAttendeeHandler(IAttendeeRepository attendeeRepo)
        {
            _attendeeRepo = attendeeRepo;
        }

        public async Task Handle(int id)
        {
            await _attendeeRepo.DeleteAsync(id);
        }
    }
}
