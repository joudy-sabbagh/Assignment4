using MediatR;

namespace Application.UseCases.Attendees
{
    public class DeleteAttendeeCommand : IRequest
    {
        public int Id { get; }

        public DeleteAttendeeCommand(int id)
        {
            Id = id;
        }
    }
}
