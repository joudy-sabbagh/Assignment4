using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class VenueRepository : IVenueRepository
{
    private readonly AppDbContext _context;

    public VenueRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Venue>> GetAllAsync()
    {
        return await _context.Venues.Include(v => v.Events).ToListAsync();
    }

    public async Task<Venue?> GetByIdAsync(int id)
    {
        return await _context.Venues.Include(v => v.Events).FirstOrDefaultAsync(v => v.VenueId == id);
    }

    public async Task AddAsync(Venue venue)
    {
        _context.Venues.Add(venue);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Venue venue)
    {
        _context.Venues.Update(venue);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var venue = await _context.Venues.FindAsync(id);
        if (venue != null)
        {
            _context.Venues.Remove(venue);
            await _context.SaveChangesAsync();
        }
    }
}
