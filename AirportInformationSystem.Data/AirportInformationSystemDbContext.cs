using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using AirportInformationSystem.Data.Models;

namespace AirportInformationSystem.Data
{
    public class AirportInformationSystemDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public AirportInformationSystemDbContext()
        {
            
        }

        public AirportInformationSystemDbContext(DbContextOptions<AirportInformationSystemDbContext> options)
            : base(options)
        {

        }

        public DbSet<Flight> Flights { get; set; } = null!;

        public DbSet<FlightRoute> FlightRoutes { get; set; } = null!;

        public DbSet<AirplaneType> AirplaneTypes { get; set; } = null!;

        public DbSet<Company> Companies { get; set; } = null!;

        public DbSet<City> Cities { get; set; } = null!;

        public DbSet<Country> Countries { get; set; } = null!;

        public DbSet<Ticket> Tickets { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FlightRoute>()
               .HasOne(fr => fr.DepartureCity)
               .WithMany(c => c.DepartureRoutes)
               .HasForeignKey(fr => fr.DepartureCityId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FlightRoute>()
                .HasOne(fr => fr.ArrivalCity)
                .WithMany(c => c.ArrivalRoutes)
                .HasForeignKey(fr => fr.ArrivalCityId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Flight>()
                .HasIndex(f => f.FlightCode)
                .IsUnique();

            modelBuilder.Entity<Ticket>()
                .HasIndex(t => new {t.UserId, t.FlightId})
                .IsUnique();

            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
