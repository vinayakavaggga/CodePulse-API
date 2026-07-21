## Database Setup (Entity Framework Core - Code First)

This project uses Entity Framework Core Code First.

### Prerequisites

- .NET 8 SDK
- SQL Server
- Visual Studio 2022 or later

### Steps to Create the Database

1. Clone the repository.

```bash
git clone <repository-url>
```

2. Update the connection string in `appsettings.json`.

Example:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=DbName;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

3. Open the project in Visual Studio.

4. Open **Tools → NuGet Package Manager → Package Manager Console**.

5. Run the following command:

```powershell
Update-Database
```

This command will create the database and apply all existing Entity Framework Core migrations.

### If the Database Already Exists

If the database already exists, `Update-Database` will apply only the pending migrations.

---

## Tech Stack

- ASP.NET Core 8 Web API
- Entity Framework Core
- SQL Server (SSMS)
- Repository Pattern
