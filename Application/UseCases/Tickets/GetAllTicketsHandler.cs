// Application/UseCases/Tickets/GetAllTicketsHandler.cs
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Application.DTOs;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Tickets
{
    public class GetAllTicketsHandler
        : IRequestHandler<GetAllTicketsQuery, List<TicketListDTO>>
    {
        private readonly ITicketRepository _ticketRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllTicketsHandler> _logger;

        public GetAllTicketsHandler(
            ITicketRepository ticketRepo,
            IMapper mapper,
            ILogger<GetAllTicketsHandler> logger)
        {
            _ticketRepo = ticketRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<TicketListDTO>> Handle(
            GetAllTicketsQuery request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllTicketsQuery");

            var entities = await _ticketRepo.GetAllAsync();
            var q = entities.AsQueryable();

            if (request.EventFilter.HasValue)
                q = q.Where(t => t.EventId == request.EventFilter.Value);

            if (!string.IsNullOrEmpty(request.CategoryFilter))
                q = q.Where(t => t.Category.ToString() == request.CategoryFilter);

            q = request.SortOrder == "price_desc"
                ? q.OrderByDescending(t => t.Price)
                : q.OrderBy(t => t.Price);

            var dtos = _mapper.Map<List<TicketListDTO>>(q.ToList());
            _logger.LogInformation("Retrieved {Count} tickets", dtos.Count);

            return dtos;
        }
    }
}
