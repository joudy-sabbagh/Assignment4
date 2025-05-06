using MediatR;

namespace Application.UseCases.Events
{
    public class DeleteEventCommand : IRequest
    {
        public int Id { get; }

        public DeleteEventCommand(int id)
        {
            Id = id;
        }
    }
}
