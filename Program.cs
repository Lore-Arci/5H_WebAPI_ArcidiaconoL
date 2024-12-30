using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Using DI for inject the db 
builder.Services.AddDbContext<HotelContext>(options => 
options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Using JSONEnumConverter from integer to string
builder.Services.AddControllers()
.AddJsonOptions(options => {
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();


// Using scope for lifetime objects to intialize the db only one time 
using (var scope = app.Services.CreateScope()) {
    var services = scope.ServiceProvider; 
    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// READ Bookings (GET)
app.MapGet("/bookings", async (HotelContext db) => {
    var bookings = await db.Bookings
                    .Include(p => p.Room)
                    .Include(p => p.Client)
                    .ToListAsync(); 

    return Results.Ok(bookings);                 
})
.WithName("GetBookings")
.WithOpenApi(); 


// READ Room (GET Filtered)
app.MapGet("/room/{roomType:int}", async (RoomType roomType, HotelContext db) => {
    if(!Enum.IsDefined(typeof(RoomType), roomType)) return Results.BadRequest("Room type not valid."); 
    

    var rooms = await db.Rooms
                            .Where(r => r.RoomType == roomType)
                            .ToListAsync(); 

    if(rooms is null) return Results.NotFound();

    return Results.Ok(rooms); 
})
.WithName("GetRoomsFiltered")
.WithOpenApi(); 

// Delete booking (DELETE)
app.MapDelete("/bookings/{id:int}", async (int id, HotelContext db) => {
    // Checking ID's validity
    if (id <= 0) {
        return Results.BadRequest(
            new { 
                error = "Invalid booking ID. Must be a positive number." 
            }
        );
    }

    var booking = await db.Bookings.FindAsync(id); 
    if(booking is null) return Results.NotFound(); 

    db.Bookings.Remove(booking); 

    await db.SaveChangesAsync();

    return Results.NoContent(); 
})
.WithName("DeleteBooking")
.WithOpenApi(); 

// Modifying Room (PUT)
app.MapPut("/rooms/{id:int}", async (int id, Room room, HotelContext db) => {
    // Checking ID's validity
    if (id <= 0) {
        return Results.BadRequest(
            new { 
                error = "Invalid room ID. Must be a positive number." 
            }
        );
    }
    var roomReturned = await db.Rooms.FindAsync(id); 
    if(roomReturned is null) return Results.NotFound(); 

    roomReturned.IsAvaiable = room.IsAvaiable; 
    roomReturned.NightPrice = room.NightPrice; 
    roomReturned.RoomType = room.RoomType; 

    await db.SaveChangesAsync(); 

    return Results.NoContent(); 
})
.WithName("PutRoomsModifyng")
.WithOpenApi(); 

// READ record (DTO READ)
app.MapGet("/booking", async (HotelContext db) => {
    // Takings bookings with even id
    var bookings = await db.Bookings
                        .Include(b => b.Client)
                        .Include(b => b.Room)
                        //.Where(b => b.BookingId % 2 == 0)
                        .ToListAsync(); 
    if(bookings is null) return Results.NotFound(); 

    var vista = bookings.Select(b => new BookingInfoResume(
        CheckInDate: b.CheckInDate, 
        CheckOut: b.CheckOutDate, 
        ClientName: b.Client.Name, 
        ClientSurname: b.Client.Surname, 
        RoomNumber: b.Room.RoomNumber,
        RoomType: b.Room.RoomType
    )); 

    return Results.Ok(vista); 
})
.WithName("GetBookingInfoResume")
.WithOpenApi(); 

// Creating Bookings (POST DTO)
app.MapPost("/booking_room", async (CreateRoomAndBooking createRoomAndBooking, HotelContext db) => {
    try {
        // Check if the RoomNumber already exists
        var existingRoom = await db.Rooms.FirstOrDefaultAsync(r => r.RoomNumber == createRoomAndBooking.RoomNumber);
        if (existingRoom != null) {
            return Results.BadRequest(new { error = "Room with this RoomNumber already exists." });
        }

        // Room that will be added to the booking
        var room = new Room{
            RoomNumber = createRoomAndBooking.RoomNumber, 
            RoomType = (RoomType)createRoomAndBooking.RoomType, 
            NightPrice = createRoomAndBooking.NightPrice, 
            IsAvaiable = createRoomAndBooking.IsAvaiable
        };

        var booking = new Booking{
            CheckInDate = createRoomAndBooking.CheckInDate, 
            CheckOutDate = createRoomAndBooking.CheckOutDate,
            // Total amount equals to check-in - check-out dates (in days), * nightprice
            TotalAmount = (createRoomAndBooking.CheckOutDate - createRoomAndBooking.CheckInDate).Days * createRoomAndBooking.NightPrice,
            Room = room
        };

        db.Bookings.Add(booking); 
        await db.SaveChangesAsync(); 
        return Results.Created($"/booking/{booking.BookingId}", booking); 
    } catch (Exception ex) {
        return Results.BadRequest(new { error = ex.Message });
    }
})
.WithName("PostBookingAndRoom")
.WithOpenApi();

// ERROR HANDLING
// Middleware 1
app.Use( async (context, next) => {
    try{
        await next(); 
    } 
    // Client error
    catch (JsonException ex) {
        context.Response.StatusCode = StatusCodes.Status400BadRequest; 
        context.Response.ContentType = "application/json";

        var errorResponse = new
        {
            Error = "JSON format error",
            Details = ex.Message
        };

        await context.Response.WriteAsJsonAsync(errorResponse); 
    }  
    // Server error
    catch (Exception ex) {
       // Gestione di altri errori
       context.Response.StatusCode = StatusCodes.Status500InternalServerError;
       context.Response.ContentType = "application/json";
       var errorResponse = new
       {
           Error = "Server internal error",
           Details = ex.Message
       };
       await context.Response.WriteAsJsonAsync(errorResponse);
   }

}); 

// Middleware 2 (terminal one)
app.Run();

record BookingInfoResume(DateTime CheckInDate, DateTime CheckOut, string ClientName, string ClientSurname, int RoomNumber, RoomType RoomType) {

}

record CreateRoomAndBooking(int RoomNumber, int RoomType, float NightPrice, bool IsAvaiable, DateTime CheckInDate, DateTime CheckOutDate) {  
    
}

