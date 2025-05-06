using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class VenueRepository : IVenueRepository
    {
        private readonly AppDbContext _context;
        public VenueRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Venue>> GetAllAsync() =>
            await _context.Venues.ToListAsync();

        public async Task<Venue?> GetByIdAsync(int id) =>
            await _context.Venues.FindAsync(id);

        public async Task AddAsync(Venue venue)
        {
            await _context.Venues.AddAsync(venue);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Venue venue)
        {
            _context.Venues.Update(venue);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Venues.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
