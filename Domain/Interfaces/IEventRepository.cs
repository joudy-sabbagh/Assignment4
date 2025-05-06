using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllAsync();
        Task<Event?> GetByIdAsync(int id);
        Task AddAsync(Event ev);
        Task UpdateAsync(Event ev);
        Task DeleteAsync(int id);
    }
}
