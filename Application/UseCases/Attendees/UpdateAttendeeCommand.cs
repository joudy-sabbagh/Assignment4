// Application/UseCases/Attendees/UpdateAttendeeCommand.cs
using MediatR;
using Application.DTOs;

namespace Application.UseCases.Attendees
{
    // Changed from IRequest to IRequest<Unit>
    public record UpdateAttendeeCommand(UpdateAttendeeDTO Dto) : IRequest<Unit>;
}
