using Core.Entities;

namespace Core.Interfaces;

public interface IAttendeeRepository
{
    Task<IEnumerable<Attendee>> GetAllAsync();
    Task<Attendee?> GetByIdAsync(int id);
    Task AddAsync(Attendee attendee);
    Task UpdateAsync(Attendee attendee);
    Task DeleteAsync(int id);
}
