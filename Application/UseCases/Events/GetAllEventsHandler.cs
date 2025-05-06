using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Events
{
    public class GetAllEventsHandler : IRequestHandler<GetAllEventsQuery, List<Event>>
    {
        private readonly IEventRepository _eventRepo;

        public GetAllEventsHandler(IEventRepository eventRepo)
        {
            _eventRepo = eventRepo;
        }

        public async Task<List<Event>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
        {
            return await _eventRepo.GetAllAsync();
        }
    }
}
