# ResidenciasNLayer

A complete .NET 8 solution implementing a 4-layer N-layer architecture for a residence management system with PostgreSQL, JWT authentication, and Expo push notifications.

## Architecture

This solution follows a 4-layer N-layer architecture:

- **Core**: Domain entities and interfaces
- **Application**: Services, contracts, and DTOs
- **Infrastructure**: EF Core, repositories, and external integrations
- **Api**: ASP.NET Core Web API controllers and hosting

## Key Features

- ✅ JWT-based authentication and authorization
- ✅ PostgreSQL database with lowercase naming convention
- ✅ User management with roles (preceptor, tutor, guardia, residente)
- ✅ Exit request workflow management
- ✅ Push notifications via Expo EAS
- ✅ Device token management
- ✅ Attendance tracking

## Database Schema

All table and column names are lowercase to ensure PostgreSQL compatibility without quoted identifiers.

### Entities

- `user`: User accounts with roles
- `preceptortype`: Administrator/Monitor types
- `residenttype`: University/College resident types
- `shift`: Work shifts with time ranges
- `tutor`: Tutor assignments
- `preceptor`: Preceptor assignments with type and shift
- `guard`: Guard assignments with shift
- `resident`: Resident assignments with type and tutor
- `device`: FCM device tokens for push notifications
- `usertoken`: JWT token storage
- `exitrequest`: Exit request workflow management
- `event`: Events and activities
- `attendance`: Attendance tracking (daily/event-based)
- `novelty`: Incident/novelty reports

## Technology Stack

### Core Layer
- Target Framework: `net8.0`
- No external dependencies

### Application Layer
- Target Framework: `net8.0`
- Dependencies: Core layer

### Infrastructure Layer
- Target Framework: `net8.0`
- **NuGet Packages:**
  - `Microsoft.EntityFrameworkCore` (8.0.10)
  - `Microsoft.EntityFrameworkCore.Design` (8.0.10)
  - `Npgsql.EntityFrameworkCore.PostgreSQL` (8.0.4)
  - `BCrypt.Net-Next` (4.0.3)
  - `Microsoft.IdentityModel.Tokens` (8.14.0)
  - `System.IdentityModel.Tokens.Jwt` (8.14.0)
- Dependencies: Core, Application layers

### API Layer
- Target Framework: `net8.0`
- **NuGet Packages:**
  - `Microsoft.AspNetCore.Authentication.JwtBearer` (8.0.10)
  - `Microsoft.EntityFrameworkCore.Tools` (8.0.10)
  - `Swashbuckle.AspNetCore` (6.9.0)
- Dependencies: Application, Infrastructure layers

## Project References

```
Api
├── Application
│   └── Core
├── Infrastructure
    ├── Application
    └── Core
```

## Getting Started

### Prerequisites

- .NET 8 SDK
- PostgreSQL server
- (Optional) Expo development tools for push notifications

### Database Setup

1. **Update Connection String**
   Edit `Api/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=your-host;Database=residencias;Username=your-username;Password=your-password"
     }
   }
   ```

2. **Install EF Core Tools** (if not already installed)
   ```bash
   dotnet tool install --global dotnet-ef
   ```

3. **Run Database Migrations**
   ```bash
   # From solution root directory
   dotnet ef database update --project Infrastructure --startup-project Api
   ```

4. **Seed Lookup Data**
   The application will automatically seed lookup tables (preceptortypes, residenttypes, shifts) on startup.

### Running the Application

1. **Restore Dependencies**
   ```bash
   dotnet restore
   ```

2. **Build Solution**
   ```bash
   dotnet build
   ```

3. **Run API**
   ```bash
   cd Api
   dotnet run
   ```

4. **Access Swagger UI**
   Navigate to `https://localhost:5001/swagger` (or the port shown in console)

## Configuration

### JWT Settings

Update `Api/appsettings.json`:
```json
{
  "JWT": {
    "SecretKey": "your-secret-key-at-least-32-characters-long",
    "Issuer": "ResidenciasNLayer",
    "Audience": "ResidenciasNLayer"
  }
}
```

### Environment Variables

For production, use environment variables instead of appsettings.json:

- `ConnectionStrings__DefaultConnection`
- `JWT__SecretKey`
- `JWT__Issuer`
- `JWT__Audience`

## API Endpoints

### Authentication
- `POST /api/auth/login` - User login
- `POST /api/auth/register` - User registration
- `POST /api/auth/device-token` - Register device for push notifications

### Exit Requests
- `GET /api/exitrequest/resident/{residentId}` - Get exit requests by resident
- `POST /api/exitrequest/resident/{residentId}` - Create exit request
- `PUT /api/exitrequest/{requestId}/authorize` - Authorize/reject exit request
- `PUT /api/exitrequest/{requestId}/exit` - Process exit
- `PUT /api/exitrequest/{requestId}/return` - Process return

### Residents
- `GET /api/resident/tutor/{tutorId}` - Get residents by tutor

## Business Rules Implementation

The solution implements the following business rules (UC-01 to UC-06):

1. **UC-01**: User authentication and JWT token generation
2. **UC-02**: Exit request creation and approval workflow
3. **UC-03**: Tutor authorization for exit requests
4. **UC-04**: Preceptor authorization for exit requests
5. **UC-05**: Guard processing of exits and returns
6. **UC-06**: Attendance tracking and reporting

## Development Notes

### Database Conventions

- All entity class names are lowercase (e.g., `user`, `exitrequest`)
- All property names are lowercase (e.g., `id`, `username`, `created_at`)
- PostgreSQL tables are explicitly mapped with `.ToTable("tablename")`
- No quoted identifiers are used to avoid PostgreSQL errors

### Validation Strategy

Following the requirement for minimal validation, the solution implements only essential validations:
- Username uniqueness
- Required fields validation
- JWT token validation
- Basic business rule enforcement

### Push Notifications

The solution integrates with Expo EAS for push notifications:
- Device tokens are stored in the `device` table
- HTTP REST API integration with Expo push service
- Automatic token management (deactivate old tokens when new ones are registered)

## Migration Commands

### Create Migration
```bash
dotnet ef migrations add MigrationName --project Infrastructure --startup-project Api
```

### Update Database
```bash
dotnet ef database update --project Infrastructure --startup-project Api
```

### Remove Last Migration
```bash
dotnet ef migrations remove --project Infrastructure --startup-project Api
```

## Contributing

1. Follow the established naming conventions (lowercase for database entities)
2. Maintain the 4-layer architecture separation
3. Keep validations minimal as per requirements
4. Ensure all database operations use lowercase naming

## License

This project is for educational/internal use.