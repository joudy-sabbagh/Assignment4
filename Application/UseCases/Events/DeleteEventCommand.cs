using MediatR;

namespace Application.UseCases.Events
{
    public class DeleteEventCommand : IRequest<Unit>
    {
        public int Id { get; }
        public DeleteEventCommand(int id) => Id = id;
    }
}
