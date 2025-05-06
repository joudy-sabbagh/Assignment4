using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class AttendeeRepository : IAttendeeRepository
    {
        private readonly AppDbContext _context;
        public AttendeeRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Attendee>> GetAllAsync() =>
            await _context.Attendees.ToListAsync();

        public async Task<Attendee?> GetByIdAsync(int id) =>
            await _context.Attendees.FindAsync(id);

        public async Task AddAsync(Attendee attendee)
        {
            await _context.Attendees.AddAsync(attendee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Attendee attendee)
        {
            _context.Attendees.Update(attendee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Attendees.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
