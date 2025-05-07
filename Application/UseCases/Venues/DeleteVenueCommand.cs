// Application/UseCases/Venues/DeleteVenueCommand.cs
using MediatR;

namespace Application.UseCases.Venues
{
    // Now implements IRequest<Unit>
    public class DeleteVenueCommand : IRequest<Unit>
    {
        public int Id { get; }
        public DeleteVenueCommand(int id) => Id = id;
    }
}
