# BugsManager Clean Architecture Web API whit MVC 
This repository contains the source code for the BugsManager API, built with ASP.NET Core 7 and SQL Server.

## Clone the repository:

 ```bash
   git clone https://github.com/dernys/BugsManager.git Bugs
   cd Bugs
```

## Build
To build the project, you need to have the .NET SDK installed. Run the following command in the project's root directory:

```bash
dotnet build
```

This command will build the project and its dependencies.

## Database Migrations

Before running the database update, make sure to configure the database connection in the appsettings.json file. 

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=Your-Sql-Server;Database=Your-Database-Name;User Id=sa;Password=Your-Password;Trusted_Connection=False;Encrypt=True;TrustServerCertificate=True;"
  }
}
```
Before running the application, make sure to apply the initial database migrations. If you are using a SQL Server database, you can use the following instructions:

Open a terminal in the project directory.
Run the migrations to create the database:

```bash
cd BugsManager.Persistence
dotnet ef database update
```

This will apply the initial migrations and configure the database according to the project's model.

## Run

After configuring the database and applying the migrations, you can run the application using the following command:

```bash
dotnet run --project BugsManager.WebAPI
dotnet run --project BugsManager.WebUI
```


