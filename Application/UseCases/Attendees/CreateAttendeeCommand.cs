using Application.DTOs;
using MediatR;

namespace Application.UseCases.Attendees
{
    public class CreateAttendeeCommand : IRequest<int>
    {
        public CreateAttendeeDTO Dto { get; }

        public CreateAttendeeCommand(CreateAttendeeDTO dto)
        {
            Dto = dto;
        }
    }
}
