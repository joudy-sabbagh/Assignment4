using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Tickets
{
    public class GetAllTicketsHandler : IRequestHandler<GetAllTicketsQuery, IEnumerable<Ticket>>
    {
        private readonly ITicketRepository _ticketRepo;

        public GetAllTicketsHandler(ITicketRepository ticketRepo)
        {
            _ticketRepo = ticketRepo;
        }

        public async Task<IEnumerable<Ticket>> Handle(GetAllTicketsQuery request, CancellationToken cancellationToken)
        {
            var ticketsQuery = _ticketRepo.GetAllWithEventAndAttendee();

            if (request.EventFilter.HasValue)
                ticketsQuery = ticketsQuery.Where(t => t.EventId == request.EventFilter.Value);

            if (!string.IsNullOrEmpty(request.CategoryFilter) &&
                Enum.TryParse<TicketCategory>(request.CategoryFilter, out var parsedCategory))
                ticketsQuery = ticketsQuery.Where(t => t.Category == parsedCategory);

            ticketsQuery = request.SortOrder switch
            {
                "price_desc" => ticketsQuery.OrderByDescending(t => (double)t.Price),
                _ => ticketsQuery.OrderBy(t => (double)t.Price)
            };

            return await ticketsQuery.ToListAsync();
        }
    }
}
