# Bibliotar 📚
BiblioTar is a library management system designed for efficient book borrowing, user management, and transaction tracking. The system is developed using .NET Core and Entity Framework Core and is intended to streamline library operations by providing a structured approach to managing books, users, and borrowing activities.


## Features

- **User Management:** Create, update, and manage user profiles.
- **Book Management:** Add, update, and delete book records.
- **Borrowing System:** Track book borrowings, returns, and overdue fines.
- **Transaction History:** View transaction logs and borrowing history.
- **Notifications:** Notify users of overdue books and fines.

## Tech Stack

- C#
- .NET Core
- Entity Framework Core
- SQL Server
- Swagger for API documentation

## Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/ImHasuy/BIBLIOTAR.git
   cd BIBLIOTAR
   ```

2. Set up the database:
   - Update the `Program.cs` with your SQL Server connection string.
   - Apply migrations:
     ```bash
     dotnet ef database update
     ```

3. Run the application:
   ```bash
   dotnet run
   ```

4. Access the Swagger documentation:
   - Navigate to `http://localhost:5000/swagger` to test API endpoints.

## API Endpoints

- **/api/users** - User management endpoints
- **/api/books** - Book management endpoints
- **/api/borrows** - Borrowing management endpoints
