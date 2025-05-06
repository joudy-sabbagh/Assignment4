using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Attendees
{
    public class GetAllAttendeesHandler : IRequestHandler<GetAllAttendeesQuery, List<Attendee>>
    {
        private readonly IAttendeeRepository _attendeeRepo;

        public GetAllAttendeesHandler(IAttendeeRepository attendeeRepo)
        {
            _attendeeRepo = attendeeRepo;
        }

        public async Task<List<Attendee>> Handle(GetAllAttendeesQuery request, CancellationToken cancellationToken)
        {
            return await _attendeeRepo.GetAllAsync();
        }
    }
}
