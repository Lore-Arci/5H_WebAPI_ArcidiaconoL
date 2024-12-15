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

// READ Clients 
app.MapGet("/clients", async (HotelContext db) => {
    var clients = await db.Clients.ToListAsync(); 

    return clients; 
})
.WithName("GetClients")
.WithOpenApi(); 


app.Run();