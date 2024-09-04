# WebDatabaseEditor - A Web-Based Database Management System

## Description

**WebDatabaseEditor** is a web-based application designed for managing and editing databases through a user-friendly interface. This project is built using ASP.NET Core with Razor Pages and Entity Framework Core, providing robust authentication and authorization mechanisms using ASP.NET Core Identity. The application also supports role-based access control, making it suitable for multi-user environments where administrators can manage users and their permissions effectively.

## Features
- **User Authentication and Authorization:** Implements cookie-based authentication with ASP.NET Core Identity, ensuring secure login, logout, and access control.
- **Role Management:** Supports role-based access using custom roles defined within the application, with functionality for managing user roles.
- **Database Management:** Provides tools for interacting with and managing SQL Server databases, including creating, updating, and deleting records.
- **Admin Setup:** Includes an admin setup service to initialize and configure the application with necessary admin roles and users.
- **Razor Pages:** Uses Razor Pages for the web interface, allowing a clean separation of concerns between the UI and backend logic.
- **Entity Framework Core:** Utilizes EF Core for database access, with support for migrations and database seeding.
- **Logging:** Integrated logging support via console and debug outputs for easy monitoring and debugging.
- **Cross-Origin Resource Sharing (CORS):** Configured to allow flexible interaction with external resources.

## Technologies Used
- .NET 8.0
- ASP.NET Core 8.0
- Entity Framework Core 8.0
- SQL Server
- Docker (Windows containers)

## Project Structure
- **Controllers:** Contains the logic for handling HTTP requests and interacting with the database.
- **Database:** Includes the database context, services, and initial setup scripts for managing the database.
- **Migrations:** Contains EF Core migration files for managing database schema changes.
- **Models:** Defines the data models used throughout the application.
- **Pages:** Razor Pages for the user interface, including account management, error handling, and shared layouts.

## Screenshots
### Home Page
![изображение](https://github.com/user-attachments/assets/1238b4c0-a36b-47c1-a902-fc6c4161a92f)


### Edit Page
![изображение](https://github.com/user-attachments/assets/0986af1e-8d8b-4b21-a474-5599faa915aa)


### User Page
![изображение](https://github.com/user-attachments/assets/776c929d-30a7-4ba8-99b8-92014dedaf3e)



## Getting Started
1. Clone the repository.
2. Build the project using the .NET SDK: `dotnet build`
3. Run the application: `dotnet run`
4. Access the application in your browser at `https://localhost:5001`.

## Usage Example
```csharp
var scope = app.Services.CreateAsyncScope();
var adminSetup = scope.ServiceProvider.GetRequiredService<AdminSetup>();
await adminSetup.SetupAdminAsync();
```
## Contributing
Contributions are welcome! Please fork the repository, create a feature branch, and submit a pull request.

## Dependencies
- Microsoft.AspNetCore.Authentication.Negotiate
- Microsoft.AspNetCore.Identity.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools
