using Microsoft.EntityFrameworkCore;

public class HotelContext : DbContext {
    public DbSet<Client> Clients { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Room> Rooms { get; set; }

    public HotelContext(DbContextOptions<HotelContext> options) : base(options) {
        
    }
}