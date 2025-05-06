using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AttendeeRepository : IAttendeeRepository
{
    private readonly AppDbContext _context;

    public AttendeeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Attendee>> GetAllAsync()
    {
        return await _context.Attendees
            .Include(a => a.Tickets)
            .ToListAsync();
    }

    public async Task<Attendee?> GetByIdAsync(int id)
    {
        return await _context.Attendees
            .Include(a => a.Tickets)
            .FirstOrDefaultAsync(a => a.AttendeeId == id);
    }

    public async Task AddAsync(Attendee attendee)
    {
        _context.Attendees.Add(attendee);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Attendee attendee)
    {
        _context.Attendees.Update(attendee);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var attendee = await _context.Attendees.FindAsync(id);
        if (attendee != null)
        {
            _context.Attendees.Remove(attendee);
            await _context.SaveChangesAsync();
        }
    }
}
