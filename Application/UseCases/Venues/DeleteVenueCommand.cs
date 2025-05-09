using MediatR;

namespace Application.UseCases.Venues
{
    public class DeleteVenueCommand : IRequest<Unit>
    {
        public int Id { get; }
        public DeleteVenueCommand(int id) => Id = id;
    }
}
