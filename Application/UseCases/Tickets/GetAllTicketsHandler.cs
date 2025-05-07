using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Tickets
{
    public class GetAllTicketsHandler : IRequestHandler<GetAllTicketsQuery, List<Ticket>>
    {
        private readonly ITicketRepository _ticketRepo;

        public GetAllTicketsHandler(ITicketRepository ticketRepo)
        {
            _ticketRepo = ticketRepo;
        }

        public async Task<List<Ticket>> Handle(GetAllTicketsQuery request, CancellationToken cancellationToken)
        {
            var tickets = (await _ticketRepo.GetAllWithEventAndAttendeeAsync()).ToList();

            if (request.EventFilter.HasValue)
                tickets = tickets.Where(t => t.EventId == request.EventFilter.Value).ToList();

            if (!string.IsNullOrEmpty(request.CategoryFilter) &&
                Enum.TryParse<TicketCategory>(request.CategoryFilter, out var cat))
            {
                tickets = tickets.Where(t => t.Category == cat).ToList();
            }

            tickets = request.SortOrder == "price_desc"
                ? tickets.OrderByDescending(t => t.Price).ToList()
                : tickets.OrderBy(t => t.Price).ToList();

            return tickets;
        }
    }
}