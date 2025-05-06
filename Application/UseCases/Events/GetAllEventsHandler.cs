using System.Collections.Generic;      // for List<T>
using System.Linq;                     // for .ToList()
using System.Threading;                // for CancellationToken
using System.Threading.Tasks;          // for Task<T>
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
            return (await _eventRepo.GetAllAsync()).ToList();

        }
    }
}
