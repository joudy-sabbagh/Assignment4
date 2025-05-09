using MediatR;
using Microsoft.Extensions.Logging;
using Domain.Interfaces;
using Application.UseCases.Attendees;

namespace Application.UseCases.Attendees
{
    public class DeleteAttendeeHandler : IRequestHandler<DeleteAttendeeCommand, Unit>
    {
        private readonly IAttendeeRepository _attendeeRepo;
        private readonly ILogger<DeleteAttendeeHandler> _logger;

        public DeleteAttendeeHandler(
            IAttendeeRepository attendeeRepo,
            ILogger<DeleteAttendeeHandler> logger)
        {
            _attendeeRepo = attendeeRepo;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteAttendeeCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting attendee with Id {Id}", request.Id);
            await _attendeeRepo.DeleteAsync(request.Id);
            _logger.LogInformation("Deleted attendee with Id {Id}", request.Id);
            return Unit.Value;
        }
    }
}
