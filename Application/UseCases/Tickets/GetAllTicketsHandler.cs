using System.Collections.Generic;      // for List<T>
using System.Linq;                     // for .ToList()
using System.Threading;                // for CancellationToken
using System.Threading.Tasks;          // for Task<T>
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            // 1) Get the base queryable (with includes)
            var ticketsQuery = _ticketRepo.GetAllWithEventAndAttendee();

            // 2) Filters
            if (request.EventFilter.HasValue)
                ticketsQuery = ticketsQuery.Where(t => t.EventId == request.EventFilter.Value);

            if (!string.IsNullOrEmpty(request.CategoryFilter) &&
                Enum.TryParse<TicketCategory>(request.CategoryFilter, out var parsedCategory))
            {
                ticketsQuery = ticketsQuery.Where(t => t.Category == parsedCategory);
            }

            // 3) Sorting
            ticketsQuery = request.SortOrder == "price_desc"
                ? ticketsQuery.OrderByDescending(t => (double)t.Price)
                : ticketsQuery.OrderBy(t => (double)t.Price);

            // 4) Materialize asynchronously to List<Ticket>
            return await ticketsQuery.ToListAsync();
        }
    }
}
