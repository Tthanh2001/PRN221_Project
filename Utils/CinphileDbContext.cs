using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PRN221_Project.Models;
using PRN221_Project.Models;

namespace PRN221_Project.Utils
{
    public class CinphileDbContext : IdentityDbContext<ApplicationAccount>
    {
        public DbSet<Bill> Bills { get; set; } = null!;
        public DbSet<Movie> Movies { get; set; } = null!;
        public DbSet<MovieSchedule> MovieSchedules { get; set; } = null!;
        public DbSet<Rating> Ratings { get; set; } = null!;
        public DbSet<Room> Rooms { get; set; } = null!;
        public DbSet<Seat> Seats { get; set; } = null!;
        public DbSet<SeatBooking> SeatBookings { get; set; } = null!;
        public DbSet<SeatType> SeatTypes { get; set; } = null!;

        public CinphileDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            base.OnConfiguring(optionsBuilder);
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MovieSchedule>()
                .HasIndex(ms => new { ms.Id, ms.StartTime })
                .IsUnique(true);

            modelBuilder.Entity<SeatBooking>()
                .HasKey(sb => new { sb.SeatId, sb.MovieScheduleId });

            modelBuilder.Entity<SeatBooking>()
                .HasOne(sb => sb.Seat)
                .WithMany(s => s.SeatBookings)
                .OnDelete(DeleteBehavior.NoAction);
           
        }


    }
}
