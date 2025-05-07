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

            if (attendee == null)
            {
                // You can throw an exception, return an error result, or handle it gracefully
                throw new Exception($"Attendee with ID {dto.AttendeeId} not found.");
            }

            attendee.Name = dto.Name;
            attendee.Email = dto.Email;
            await _attendeeRepo.UpdateAsync(attendee);
        }

    }
}
