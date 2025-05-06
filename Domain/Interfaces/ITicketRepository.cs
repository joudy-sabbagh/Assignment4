using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ITicketRepository
    {
        IQueryable<Ticket> GetAllWithEventAndAttendee();
        Task<IEnumerable<Ticket>> GetAllAsync();
        Task<Ticket?> GetByIdAsync(int id);
        Task AddAsync(Ticket ticket);
        Task UpdateAsync(Ticket ticket);
        Task DeleteAsync(int id);
    }
}
