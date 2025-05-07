// Application/UseCases/Events/DeleteEventCommand.cs
using MediatR;

namespace Application.UseCases.Events
{
    // now implements IRequest<Unit>
    public class DeleteEventCommand : IRequest<Unit>
    {
        public int Id { get; }
        public DeleteEventCommand(int id) => Id = id;
    }
}
