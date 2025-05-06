using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Attendees
{
    public class CreateAttendeeHandler : IRequestHandler<CreateAttendeeCommand, int>
    {
        private readonly IAttendeeRepository _attendeeRepo;

        public CreateAttendeeHandler(IAttendeeRepository attendeeRepo)
        {
            _attendeeRepo = attendeeRepo;
        }

        public async Task<int> Handle(CreateAttendeeCommand request, CancellationToken cancellationToken)
        {
            var attendee = new Attendee
            {
                Name = request.Dto.Name,
                Email = request.Dto.Email
            };

            await _attendeeRepo.AddAsync(attendee);
            return attendee.Id;
        }
    }
}
