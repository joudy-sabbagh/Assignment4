// Application/UseCases/Events/GetAllEventsHandler.cs
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Application.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Events
{
    public class GetAllEventsHandler
        : IRequestHandler<GetAllEventsQuery, List<EventListDTO>>
    {
        private readonly IEventRepository _repo;
        private readonly IMapper _mapper;

        public GetAllEventsHandler(
            IEventRepository repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<EventListDTO>> Handle(
            GetAllEventsQuery request,
            CancellationToken cancellationToken)
        {
            var entities = await _repo.GetAllAsync();
            return _mapper.Map<List<EventListDTO>>(entities);
        }
    }
}
