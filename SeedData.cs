using Microsoft.EntityFrameworkCore;

public class SeedData {
    public static void Initialize(IServiceProvider serviceProvider) {
        using (var context = new HotelContext(options: serviceProvider.GetRequiredService<DbContextOptions<HotelContext>>())) {
            if(!context.Clients.Any() || !context.Bookings.Any() || !context.Rooms.Any()) {
                // Rooms
                var room1 = new Room
                {
                    RoomNumber = 101,
                    RoomType = RoomType.Single, // 0
                    NightPrice = 100.0f,  
                    IsAvailable = true
                };

                var room2 = new Room
                {
                    RoomNumber = 102,
                    RoomType = RoomType.Double, // 1
                    NightPrice = 150.0f,  
                    IsAvailable = true
                };

                var room3 = new Room
                {
                    RoomNumber = 201,
                    RoomType = RoomType.Suite, 
                    NightPrice = 250.0f,  
                    IsAvailable = true
                };

                var room4 = new Room
                {
                    RoomNumber = 202,
                    RoomType = RoomType.Junior, 
                    NightPrice = 400.0f,  
                    IsAvailable = false  
                };

                // Add Rooms to the "Rooms table"
                context.Rooms.AddRange(room1, room2, room3, room4);

                // Clients
                var client1 = new Client{
                    Name = "John",
                    Surname = "Doe",
                    Email = "john.doe@example.com",
                    PhoneNumber = new PhoneNumber("1", "1234567890"), 
                    Address = "123 Elm Street, Springfield, IL"
                };

                var client2 = new Client{
                    Name = "Jane",
                    Surname = "Smith",
                    Email = "jane.smith@example.com",
                    PhoneNumber = new PhoneNumber("1", "9876543210"), 
                    Address = "456 Oak Avenue, Springfield, IL"
                };

                var client3 = new Client{
                    Name = "Michael",
                    Surname = "Johnson",
                    Email = "michael.johnson@example.com",
                    PhoneNumber = new PhoneNumber("44", "5551234567"),
                    Address = "789 Pine Road, Springfield, IL"
                };

                var client4 = new Client{
                    Name = "Emily",
                    Surname = "Davis",
                    Email = "emily.davis@example.com",
                    PhoneNumber = new PhoneNumber("61", "5559876543"),
                    Address = "101 Maple Lane, Springfield, IL"
                };

                // Adding clients to "Clients table"
                context.Clients.AddRange(client1, client2, client3, client4);

                // Adding bookings to "Bookings table"
                context.Bookings.AddRange(
                    new Booking
                    {
                        CheckInDate = new DateTime(2024, 12, 15),
                        CheckOutDate = new DateTime(2024, 12, 20),
                        TotalAmount = room1.NightPrice * 5,  // Example calculation based on price per night
                        Room = room1,
                        Client = client1
                    },
                    new Booking
                    {
                        CheckInDate = new DateTime(2024, 12, 16),
                        CheckOutDate = new DateTime(2024, 12, 18),
                        TotalAmount = room2.NightPrice * 2,  // Example calculation based on price per night
                        Room = room2,
                        Client = client1
                    },
                    new Booking
                    {
                        CheckInDate = new DateTime(2024, 12, 17),
                        CheckOutDate = new DateTime(2024, 12, 22),
                        TotalAmount = room3.NightPrice * 5,  // Example calculation based on price per night
                        Room = room3,
                        Client = client2
                    },
                    new Booking
                    {
                        CheckInDate = new DateTime(2024, 12, 18),
                        CheckOutDate = new DateTime(2024, 12, 25),
                        TotalAmount = room1.NightPrice * 7,  // Example calculation based on price per night
                        Room = room1,
                        Client = client2
                    }
                );

                // Save changes to the database
                context.SaveChanges();
            }
        }
    }
}