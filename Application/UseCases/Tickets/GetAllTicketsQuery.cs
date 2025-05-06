using Domain.Entities;
using MediatR;

namespace Application.UseCases.Tickets
{
    public class GetAllTicketsQuery : IRequest<IEnumerable<Ticket>>
    {
        public string SortOrder { get; }
        public int? EventFilter { get; }
        public string CategoryFilter { get; }

        public GetAllTicketsQuery(string sortOrder, int? eventFilter, string categoryFilter)
        {
            SortOrder = sortOrder;
            EventFilter = eventFilter;
            CategoryFilter = categoryFilter;
        }
    }
}
