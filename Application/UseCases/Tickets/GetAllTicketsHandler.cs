using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Application.Common;            
using Application.DTOs;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Tickets
{
    public class GetAllTicketsHandler
        : IRequestHandler<GetAllTicketsQuery, Result<List<TicketListDTO>>>
    {
        private readonly ITicketRepository _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllTicketsHandler> _logger;

        public GetAllTicketsHandler(
            ITicketRepository repo,
            IMapper mapper,
            ILogger<GetAllTicketsHandler> logger)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<List<TicketListDTO>>> Handle(
            GetAllTicketsQuery request,
            CancellationToken ct)
        {
            _logger.LogInformation(
              "Handling GetAllTicketsQuery (sort={Sort}, event={Ev}, category={Cat})",
               request.SortOrder, request.EventFilter, request.CategoryFilter);

            var tickets = await _repo.GetAllAsync();
            if (tickets == null || !tickets.Any())
            {
                _logger.LogWarning("No tickets found");
                return Result<List<TicketListDTO>>.Failure("No tickets available");
            }

            if (request.EventFilter.HasValue)
                tickets = tickets.Where(t => t.EventId == request.EventFilter.Value).ToList();
            if (!string.IsNullOrEmpty(request.CategoryFilter))
                tickets = tickets
                    .Where(t => t.Category.ToString() == request.CategoryFilter)
                    .ToList();

            tickets = request.SortOrder?.ToLower() switch
            {
                "price" => tickets.OrderBy(t => t.Price).ToList(),
                "price_desc" => tickets.OrderByDescending(t => t.Price).ToList(),
                "category" => tickets.OrderBy(t => t.Category).ToList(),
                "category_desc" => tickets.OrderByDescending(t => t.Category).ToList(),
                "event" => tickets.OrderBy(t => t.Event.Name).ToList(),
                "event_desc" => tickets.OrderByDescending(t => t.Event.Name).ToList(),
                "attendee" => tickets.OrderBy(t => t.Attendee.Name).ToList(),
                "attendee_desc" => tickets.OrderByDescending(t => t.Attendee.Name).ToList(),
                _ => tickets.OrderBy(t => t.Id).ToList(),
            };

            var dtos = _mapper.Map<List<TicketListDTO>>(tickets);
            _logger.LogInformation("Retrieved {Count} tickets", dtos.Count);
            return Result<List<TicketListDTO>>.Success(dtos);
        }
    }
}
