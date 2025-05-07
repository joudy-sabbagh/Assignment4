// Application/UseCases/Venues/DeleteVenueHandler.cs
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Venues
{
    public class DeleteVenueHandler : IRequestHandler<DeleteVenueCommand, Unit>
    {
        private readonly IVenueRepository _venueRepo;
        private readonly ILogger<DeleteVenueHandler> _logger;

        public DeleteVenueHandler(IVenueRepository venueRepo, ILogger<DeleteVenueHandler> logger)
        {
            _venueRepo = venueRepo;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteVenueCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting venue {Id}", request.Id);
            await _venueRepo.DeleteAsync(request.Id);
            _logger.LogInformation("Deleted venue {Id}", request.Id);
            return Unit.Value;
        }
    }
}
