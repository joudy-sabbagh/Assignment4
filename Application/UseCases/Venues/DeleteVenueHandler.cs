using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Venues
{
    public class DeleteVenueHandler : IRequestHandler<DeleteVenueCommand>
    {
        private readonly IVenueRepository _venueRepo;

        public DeleteVenueHandler(IVenueRepository venueRepo)
        {
            _venueRepo = venueRepo;
        }

        public async Task Handle(DeleteVenueCommand request, CancellationToken cancellationToken)
        {
            await _venueRepo.DeleteAsync(request.Id);
        }
    }
}
