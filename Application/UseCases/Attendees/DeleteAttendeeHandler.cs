using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Attendees
{
    public class DeleteAttendeeHandler : IRequestHandler<DeleteAttendeeCommand>
    {
        private readonly IAttendeeRepository _attendeeRepo;

        public DeleteAttendeeHandler(IAttendeeRepository attendeeRepo)
        {
            _attendeeRepo = attendeeRepo;
        }

        public async Task Handle(DeleteAttendeeCommand request, CancellationToken cancellationToken)
        {
            await _attendeeRepo.DeleteAsync(request.Id);
        }
    }
}
