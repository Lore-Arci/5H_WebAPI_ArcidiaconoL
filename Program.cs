using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

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

// READ Client
app.MapGet("/clients", async (HotelContext db) => {
    var clients = await db.Clients
                    .ToListAsync(); 

    return Results.Ok(clients);                 
})
.WithName("GetClients")
.WithOpenApi(); 

// POST Client
app.MapPost("/client", async(Client client, HotelContext db) => {
    await db.AddAsync(client); 
    await db.SaveChangesAsync(); 
    return Results.Created($"/client/{client.ClientId}", client); 
}) 
.WithName("CreateClient")
.WithOpenApi(); 

// READ Bookings
app.MapGet("/bookings", async (HotelContext db) => {
    var bookings = await db.Bookings
                    .Include(p => p.Room)
                    .Include(p => p.Client)
                    .ToListAsync(); 

    return Results.Ok(bookings);                 
})
.WithName("GetBookings")
.WithOpenApi(); 


// READ Filtered
app.MapGet("/room/{roomType:int}", async (RoomType roomType, HotelContext db) => {
    if(!Enum.IsDefined(typeof(RoomType), roomType)) return Results.BadRequest("Room type not valid."); 

    var avaiableBookings = await db.Rooms
                            .Where(r => r.RoomType == roomType)
                            .ToListAsync(); 

    return Results.Ok(avaiableBookings); 
})
.WithName("GetAvaibleRooms")
.WithOpenApi(); 

// Delete client
app.MapDelete("/clients/{id:int}", async (int id, HotelContext db) => {
    var client = await db.Clients.FindAsync(id); 
    if(client is null) return Results.NotFound(); 

    db.Clients.Remove(client); 

    await db.SaveChangesAsync();

    return Results.NoContent(); 
})
.WithName("DeleteClient")
.WithOpenApi(); 


// Modifying Booking
app.MapPut("/bookings/{id:int}", async (int id, Booking booking, HotelContext db) => {
    var bookingReturned = await db.Bookings.FindAsync(id); 
    if(bookingReturned is null) return Results.NotFound(); 

    bookingReturned.CheckInDate = booking.CheckInDate; 
    bookingReturned.CheckOutDate = booking.CheckOutDate; 

    await db.SaveChangesAsync(); 

    return Results.NoContent(); 
})
.WithName("PutBookingsModifyng")
.WithOpenApi(); 

// READ record with DTO
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
.WithName("GetRoomInfoResume")
.WithOpenApi(); 

// Creating Bookings and ROOM with DTO
/*
app.MapPost("/booking_room", async (CreateRoomAndBooking createRoomAndBooking, HotelContext db) => {
    // Room that will be added to the booking
    try {
        var room = new Room{
            RoomNumber = createRoomAndBooking.RoomNumber, 
            RoomType = createRoomAndBooking.RoomType, 
            NightPrice = createRoomAndBooking.NightPrice, 
            IsAvailable = createRoomAndBooking.IsAvailable
        };
    
        var booking = new Booking{
            CheckInDate = createRoomAndBooking.CheckInDate, 
            CheckOutDate = createRoomAndBooking.CheckOutDate,
            // Total amount eaquals to ckeckin - checkuot dates (in days), * nightprice
            TotalAmount = (createRoomAndBooking.CheckOutDate - createRoomAndBooking.CheckOutDate).Days * createRoomAndBooking.NightPrice,
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
*/

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
/*
record CreateRoomAndBooking(int RoomNumber, RoomType RoomType, float NightPrice, bool IsAvailable, DateTime CheckInDate, DateTime CheckOutDate) {  
    
}*/

