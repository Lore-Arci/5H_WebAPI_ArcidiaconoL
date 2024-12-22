<h1 align="center">
  <br>
  <a href="#"><img src="https://github.com/Lore-Arci/HotelWebAPI/blob/main/Imgs/NET%20WebAPI.png?raw=true" alt=".NET WebAPI" width="400"></a>
  <br>
  HotelProject WebAPI
  <br>
</h1>

<h4 align="center">A simple WebApi useful to access data to a Hotel Db.</h4>

<!-- Versions -->
<p align="center">
  <a href="https://dotnet.microsoft.com/it-it/download/dotnet/8.0">
    <img src="https://github.com/Lore-Arci/HotelWebAPI/blob/main/Imgs/DOTNETV.svg"
         alt=".NET">
  </a>

  <a href="https://dotnet.microsoft.com/it-it/download/dotnet/8.0">
    <img src="https://github.com/Lore-Arci/HotelWebAPI/blob/main/Imgs/ASPNETCoreV.svg"
         alt="ASPNET Core">
  </a>

  <a href="https://www.nuget.org/packages/Microsoft.EntityFrameworkCore">
    <img src="https://github.com/Lore-Arci/HotelWebAPI/blob/main/Imgs/EFCoreV.svg"
         alt="ASPNET Core">
  </a>
</p>

<!-- Sections -->
<p align="center">
  <a href="#about">About</a> •
  <a href="#how-to-use">How To Use</a> •
  <a href="#license">License</a>
</p>

<div align="center">
  <img src="https://github.com/Lore-Arci/HotelWebAPI/blob/main/Imgs/TryAPIGIF.gif"></img> 
</div>


## About

HotelProject is a simple C# WebAPI based project that have the focus to help understand how a C# WebAPI works. It fetches data from a SqLite database that contains dummy data. Those data are insert as <b>SeedData</b> if the db is empty (they are shown in the SeedData class). 

That is the structure of the db, inlcuded the relations: 
....

## How To Use

To clone and run this application you'll need <a href="https://git-scm.com">Git</a> and some packages about <b>Entity Framework Core</b>. 
```bash
  # Clone the repo
  $ git clone https://github.com/Lore-Arci/HotelWebAPI.git

  # Go into the repo
  cd HotelWebAPI

  # Install the EFCore packages
  $ dotnet add package Microsoft.EntityFrameworkCore.Design
  $ dotnet add package Microsoft.EntityFrameworkCore.SQLite
```

> **Note**
> This is supposed to work only if you have already installed EF Core globally
> ```bash
> $ dotnet tool install --global dotnet-ef
> ```

## License

MIT

---

> GitHub [@Lore-Arci](https://github.com/Lore-Arci) 
