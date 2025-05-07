// Application/UseCases/Tickets/GetAllTicketsHandler.cs
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Application.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Tickets
{
    public class GetAllTicketsHandler
        : IRequestHandler<GetAllTicketsQuery, List<TicketListDTO>>
    {
        private readonly ITicketRepository _ticketRepo;
        private readonly IMapper _mapper;

        public GetAllTicketsHandler(
            ITicketRepository ticketRepo,
            IMapper mapper)
        {
            _ticketRepo = ticketRepo;
            _mapper = mapper;
        }

        public async Task<List<TicketListDTO>> Handle(
            GetAllTicketsQuery request,
            CancellationToken cancellationToken)
        {
            var entities = await _ticketRepo.GetAllAsync();

            // apply filters and sort in memory
            var q = entities.AsQueryable();
            if (request.EventFilter.HasValue)
                q = q.Where(t => t.EventId == request.EventFilter.Value);
            if (!string.IsNullOrEmpty(request.CategoryFilter))
                q = q.Where(t => t.Category.ToString() == request.CategoryFilter);
            q = request.SortOrder == "price_desc"
                ? q.OrderByDescending(t => t.Price)
                : q.OrderBy(t => t.Price);

            return _mapper.Map<List<TicketListDTO>>(q.ToList());
        }
    }
}
