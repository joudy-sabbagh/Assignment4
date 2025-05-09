using MediatR;
using Application.DTOs;

namespace Application.UseCases.Attendees
{
    public record UpdateAttendeeCommand(UpdateAttendeeDTO Dto) : IRequest<Unit>;
}
