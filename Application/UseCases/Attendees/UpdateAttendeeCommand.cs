using Application.DTOs;
using MediatR;

namespace Application.UseCases.Attendees
{
    public class UpdateAttendeeCommand : IRequest
    {
        public UpdateAttendeeDTO Dto { get; }

        public UpdateAttendeeCommand(UpdateAttendeeDTO dto)
        {
            Dto = dto;
        }
    }
}
