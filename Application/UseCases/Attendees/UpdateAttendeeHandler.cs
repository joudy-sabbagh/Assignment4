using Application.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Attendees
{
    public class UpdateAttendeeHandler : IRequestHandler<UpdateAttendeeCommand>
    {
        private readonly IAttendeeRepository _attendeeRepo;

        public UpdateAttendeeHandler(IAttendeeRepository attendeeRepo)
        {
            _attendeeRepo = attendeeRepo;
        }

        public async Task Handle(UpdateAttendeeCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            var attendee = await _attendeeRepo.GetByIdAsync(dto.AttendeeId);
            attendee.Name = dto.Name;
            attendee.Email = dto.Email;
            await _attendeeRepo.UpdateAsync(attendee);
        }
    }
}
