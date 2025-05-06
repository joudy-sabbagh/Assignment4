using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly AppDbContext _context;

    public TicketRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Ticket>> GetAllAsync()
    {
        return await _context.Tickets
            .Include(t => t.Event)
            .Include(t => t.Attendee)
            .ToListAsync();
    }

    public async Task<Ticket?> GetByIdAsync(int id)
    {
        return await _context.Tickets
            .Include(t => t.Event)
            .Include(t => t.Attendee)
            .FirstOrDefaultAsync(t => t.TicketId == id);
    }

    public async Task AddAsync(Ticket ticket)
    {
        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Ticket ticket)
    {
        _context.Tickets.Update(ticket);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket != null)
        {
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
        }
    }
}
