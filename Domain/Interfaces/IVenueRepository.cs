using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IVenueRepository
    {
        Task<IEnumerable<Venue>> GetAllAsync();
        Task<Venue?> GetByIdAsync(int id);
        Task AddAsync(Venue venue);
        Task UpdateAsync(Venue venue);
        Task DeleteAsync(int id);
    }
}
