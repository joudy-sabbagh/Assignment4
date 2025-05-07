// Application/UseCases/Attendees/DeleteAttendeeCommand.cs
using MediatR;

namespace Application.UseCases.Attendees
{
    // Changed from IRequest to IRequest<Unit>
    public record DeleteAttendeeCommand(int Id) : IRequest<Unit>;
}
