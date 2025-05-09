using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext
        : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        public DbSet<Attendee> Attendees { get; set; } = null!;
        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<Ticket> Tickets { get; set; } = null!;
        public DbSet<Venue> Venues { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Attendee>().HasKey(a => a.Id);
            modelBuilder.Entity<Event>().HasKey(e => e.Id);
            modelBuilder.Entity<Ticket>().HasKey(t => t.Id);
            modelBuilder.Entity<Venue>().HasKey(v => v.Id);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.Venue)
                .WithMany(v => v.Events)
                .HasForeignKey(e => e.VenueId);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Event)
                .WithMany(e => e.Tickets)
                .HasForeignKey(t => t.EventId);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Attendee)
                .WithMany(a => a.Tickets)
                .HasForeignKey(t => t.AttendeeId);

            modelBuilder.Entity<Attendee>()
                .Property(a => a.Email)
                .HasConversion(
                    e => e.Value,
                    v => new EmailAddress(v))
                .HasColumnName("Email")
                .IsRequired();

            modelBuilder.Entity<Ticket>()
                .Property(t => t.Price)
                .HasConversion(
                    m => m.Amount,
                    v => new Money(v))
                .HasColumnName("Price")
                .IsRequired();
        }
    }
}
