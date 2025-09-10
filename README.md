# ResidenciasNLayer

A .NET 8 N-Layer architecture implementation with PostgreSQL and Entity Framework Core, featuring Device and User entities with proper relationships.

## Architecture

The solution follows N-Layer architecture pattern with these projects:

- **ResidenciasNLayer.Entities** - Domain entities with lowercase properties for PostgreSQL compatibility
- **ResidenciasNLayer.Data** - Data access layer with Entity Framework Core and PostgreSQL
- **ResidenciasNLayer.Services** - Business logic layer with service interfaces and implementations
- **ResidenciasNLayer.Api** - Web API layer with controllers and dependency injection

## Key Features

### Device Entity
- `deviceid` (string) - Primary Key
- `tokenfcm` (string) - Firebase Cloud Messaging token
- `estado` (bool) - Device status, defaults to true

### User Entity
- `userid` (int) - Primary Key (auto-generated)
- `username` (string) - User name
- `email` (string) - User email
- `deviceid` (string?) - Foreign Key reference to Device

### Database Schema
- All table names and column names are lowercase for PostgreSQL compatibility
- Tables: `device`, `user`
- 1:1 relationship between User and Device using `deviceid` as the principal key
- Foreign key constraint with ON DELETE SET NULL behavior

## Getting Started

### Prerequisites
- .NET 8 SDK
- PostgreSQL database

### Setup
1. Clone the repository
2. Update connection string in `appsettings.json`
3. Run migrations: `dotnet ef database update` (from the Api project directory)
4. Run the application: `dotnet run` (from the Api project directory)

### API Endpoints
- `GET /api/devices` - Get all devices
- `GET /api/devices/{deviceid}` - Get device by ID
- `POST /api/devices` - Create new device
- `PUT /api/devices/{deviceid}` - Update device
- `DELETE /api/devices/{deviceid}` - Delete device

- `GET /api/users` - Get all users
- `GET /api/users/{userid}` - Get user by ID
- `GET /api/users/by-device/{deviceid}` - Get user by device ID
- `POST /api/users` - Create new user
- `PUT /api/users/{userid}` - Update user
- `DELETE /api/users/{userid}` - Delete user

## Testing

The application includes automatic tests that run on startup to verify:
- Entity creation and relationships
- Service layer functionality
- Database operations

## Technologies Used
- .NET 8
- Entity Framework Core 9.0
- PostgreSQL with Npgsql provider
- ASP.NET Core Web API
- Swagger/OpenAPI for documentation