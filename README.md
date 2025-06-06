<h1 align="center">
  <br>
  <a href="#"><img src="https://github.com/Lore-Arci/HotelWebAPI/blob/main/Imgs/NET%20WebAPI.png?raw=true" alt=".NET WebAPI" width="500"></a>
  <br>
  HotelProject WebAPI
  <br>
</h1>

<h4 align="center">A simple WebApi useful to access data to a Hotel Db.</h4>

<!-- Versions -->
<p align="center">
  <a href="https://dotnet.microsoft.com/it-it/download/dotnet/8.0">
    <img height="30" src="https://github.com/Lore-Arci/HotelWebAPI/blob/main/Imgs/DOTNETV.svg"
         alt=".NET">
  </a>

  <a href="https://dotnet.microsoft.com/it-it/download/dotnet/8.0">
    <img height="30" src="https://github.com/Lore-Arci/HotelWebAPI/blob/main/Imgs/ASPNETCoreV.svg"
         alt="ASPNET Core">
  </a>

  <a href="https://www.nuget.org/packages/Microsoft.EntityFrameworkCore">
    <img height="30" src="https://github.com/Lore-Arci/HotelWebAPI/blob/main/Imgs/EFCoreV.svg"
         alt="ASPNET Core">
  </a>
</p>

<!-- Sections -->
<p align="center">
  <a href="#about">About</a> •
  <a href="#how-to-use">How To Use</a> •
  <a href="#license">Details</a> •
  <a href="#license">License</a> 
</p>


## About

**HotelProject** is a simple C# WebAPI-based project designed to help understand how a C# WebAPI works. It fetches data from a SQLite database that contains dummy data. If the database is empty, the data is inserted as **SeedData**.

---

### Database Structure
Below is the structure of the database, including its relations:

<br>

<div>
  <img width="250" src="https://github.com/Lore-Arci/HotelWebAPI/blob/main/Imgs/ClientsTable.png" alt="Clients Table">
  <img width="250" src="https://github.com/Lore-Arci/HotelWebAPI/blob/main/Imgs/RoomsTable.png" alt="Rooms Table">
  <img width="250" src="https://github.com/Lore-Arci/HotelWebAPI/blob/main/Imgs/BookingsTable.png" alt="Bookings Table">
</div>

<br>

#### Generic Relations:
- **Client → Bookings (1:N)**
- **Room → Bookings (1:N)**

#### SQL Relations:
- **Client FK**: Inserted in the `Bookings` table, referring to `ClientId` (PK).
- **Room FK**: Inserted in the `Bookings` table, referring to `RoomId` (PK).

#### EF Relations:
- `Client` and `Room` have a Collection Navigation Property:
  ```csharp
  public virtual ICollection<Booking> Bookings { get; set; }

- `Booking` have Reference Navigation Properties:
  ```csharp
  public Room? Room { get; set; }
  public Client? Client { get; set; }

---

#### Database Auto-Population with SeedData

The database is automatically populated when the application is run, if any of the tables are empty. This behavior is managed by the `SeedData` class, which initializes data by using a `ServiceProvider` passed to the database options.

#### How It Works:

1. **SeedData Initialization**  
   The `SeedData.Initialize` method is responsible for initializing the database with default data. It gets the `DbContextOptions<HotelContext>` from the `IServiceProvider` and uses it to create an instance of the `HotelContext`.

   Here’s the method that gets called in the `SeedData` class:
   
   ```csharp
   public static void Initialize(IServiceProvider serviceProvider) {
       using (var context = new HotelContext(
           options: serviceProvider.GetRequiredService<DbContextOptions<HotelContext>>())) {
           
       }
   }

2. **Populating the Database**
  Within the Initialize method, after creating the HotelContext, the method proceeds to populate the database by adding instances of entities (like Client, Room, etc.) to the appropriate DbSet.

3. **Calling SeedData in Program Initialization**
   The Initialize method is called in the Program.cs file during the application startup, inside a scoped service provider. This ensures that services are properly passed to the SeedData method, which can then interact with the database.
The code for initializing the seed data looks like this:
    ```csharp
    using (var scope = app.Services.CreateScope()) {
        var services = scope.ServiceProvider; 
        SeedData.Initialize(services); // Call SeedData to populate the database
    }
    ```

## How To Use

To clone and run this application you'll need <a href="https://git-scm.com">Git</a> and some packages about <b>Entity Framework Core</b>. 
```bash
  # Clone the repo
  $ git clone https://github.com/Lore-Arci/5H_WebAPI_ArcidiaconoL.git

  # Go into the repo
  cd 5H_WebAPI_ArcidiaconoL
```

> **Note**
> This is supposed to work only if you have already installed EF Core globally
> ```bash
> $ dotnet tool install --global dotnet-ef
> ```
**To go on with the set up, follow the "Details" section down below ⬇️ .**

## Details
The filtered GET method accepts an integer instead of RoomType as it's a personalized type. 
Those are the integers corrispondences with the enum values: 
- Single -> 0
- Double -> 1
- Junior -> 2
- Suite  -> 3

The Entities of the Hotel db are empty so that you can you check the work of the SeedData class. 
If you want to retry the SeedData db initialization you should open the db with a tool for editing Sqlite files and run the following commands: 
```sql
# Making the entities empty
DELETE FROM Clients
DELETE FROM Rooms
DELETE FROM Bookings

# (Optional) Restarting the sequences of the entities (the id will be resetted to 1)
DELETE FROM sqlite_sequence WHERE name = 'Clients';
DELETE FROM sqlite_sequence WHERE name = 'Rooms';
DELETE FROM sqlite_sequence WHERE name = 'Bookings';
```


## License

MIT

---

> GitHub [@Lore-Arci](https://github.com/Lore-Arci) 
