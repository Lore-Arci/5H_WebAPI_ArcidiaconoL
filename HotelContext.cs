using Microsoft.EntityFrameworkCore;

public class HotelContext : DbContext {
    public DbSet<Client> Clients { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Room> Rooms { get; set; }

    public HotelContext(DbContextOptions<HotelContext> options) : base(options) {
        
    }

    // Creating a value converter
    // This function is called once per app lifecycle (model creation)
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Client entity
        modelBuilder.Entity<Client>(entity => {
            entity.Property(e => e.PhoneNumber)
                .HasConversion(
                    phoneNumber => phoneNumber.ToString(), // Convert PhoneNumber to string      (Serialization)
                    value => PhoneNumber.Parse(value));    // Convert string back to PhoneNumber (Deserialization)
        });

        // Room entity
        modelBuilder.Entity<Room>(entity => {
            entity.Property(e => e.RoomType)
                .HasConversion(
                    /* 
                        Serialization
                        * Enum will be converted from enumtype to string (JSON->DB)
                    
                        Deserialization
                        * Db enum will be converted from string to enumtype
                    */ 
                    roomType => roomType.ToString(),                            // Serialization
                    value => (RoomType)Enum.Parse(typeof(RoomType), value)      // Deserialization
                ); 
        }); 

        base.OnModelCreating(modelBuilder);
    }
}