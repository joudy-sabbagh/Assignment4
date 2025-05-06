using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly AppDbContext _context;
        public TicketRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Ticket>> GetAllAsync() =>
            await _context.Tickets.ToListAsync();

        // Expose a queryable for advanced filtering/sorting with joins
        public IQueryable<Ticket> GetAllWithEventAndAttendee() =>
            _context.Tickets
                    .Include(t => t.Event)
                    .Include(t => t.Attendee);

        public async Task<Ticket?> GetByIdAsync(int id) =>
            await _context.Tickets.FindAsync(id);

        public async Task AddAsync(Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Tickets.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
