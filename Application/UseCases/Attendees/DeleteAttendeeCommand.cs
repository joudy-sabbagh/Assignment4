using MediatR;

namespace Application.UseCases.Attendees
{
    public record DeleteAttendeeCommand(int Id) : IRequest<Unit>;
}
