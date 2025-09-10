# ResidenciasNLayer

## Descripción

Sistema de gestión de residencias estudiantiles desarrollado con arquitectura N-Layer usando ASP.NET Core y PostgreSQL.

## Características

- **Autenticación JWT**: Sistema seguro de autenticación y autorización basado en roles
- **Gestión de Usuarios**: Registro, login y actualización de tokens de notificación push
- **Gestión de Eventos**: Creación de eventos por preceptores con notificaciones automáticas
- **Notificaciones Push**: Integración con Expo para notificaciones móviles
- **Roles de Usuario**: preceptor, residente, guardia, tutor

## Arquitectura

El proyecto utiliza una arquitectura N-Layer limpia:

- **API Layer** (`Api/`): Controladores y configuración del Web API
- **Application Layer** (`Application/`): Servicios de aplicación, DTOs e interfaces
- **Domain Layer** (`Domain/`): Entidades del dominio
- **Infrastructure Layer** (`Infrastructure/`): Acceso a datos con Entity Framework

## Configuración

### Base de Datos

El sistema utiliza PostgreSQL. Configura la cadena de conexión en `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=Residencias;Username=postgres;Password=postgress"
  },
  "Jwt": {
    "Key": "K5$8xL@9qT#2pR7!vM3&sW6*zY4%dF1^gH0",
    "Issuer": "ResidenciasAppAPI",
    "Audience": "ResidenciasAppClients"
  }
}
```

### Notificaciones Push

El sistema almacena el token de Expo directamente en la entidad `user` en el campo `expotoken` (TEXT). 

Los tokens deben tener el formato: `ExponentPushToken[XXXXXXXXXXXXXXXXXXXXXXXXXX]`

Durante el registro, el frontend debe enviar el token de Expo que se almacenará automáticamente en el perfil del usuario.

## API Endpoints

### Autenticación

- `POST /api/auth/register` - Registro de usuario (incluye expotoken opcional)
- `POST /api/auth/login` - Inicio de sesión
- `PUT /api/auth/push-token` - Actualización de token push (requiere autenticación)

### Eventos

- `POST /api/events` - Crear evento (solo preceptores)
- `GET /api/events` - Listar eventos

## Notificaciones

### Creación de Eventos

Cuando un preceptor crea un evento, se envía automáticamente una notificación push a todos los residentes con token configurado:

- **Título**: "Nuevo evento"
- **Mensaje**: "{nombre_evento} en {fecha:yyyy-MM-dd HH:mm}"

### UC-04 - Solicitudes de Salida

Las notificaciones para solicitudes de salida se envían según el estado:

- **Nueva solicitud** → preceptores, guardias y tutor específico del residente
- **Autorización/Rechazo** → residente solicitante  
- **Salida/Regreso** → tutor del residente y preceptores asignados

## Desarrollo

### Requisitos

- .NET 8.0 SDK
- PostgreSQL 12+
- Editor compatible con C# (Visual Studio, VS Code, Rider)

### Comandos

```bash
# Restaurar dependencias
dotnet restore

# Compilar
dotnet build

# Ejecutar API
dotnet run --project Api/ResidenciasNLayer.Api.csproj

# Crear migración
dotnet ef migrations add InitialCreate --project Infrastructure/ --startup-project Api/

# Aplicar migración
dotnet ef database update --project Infrastructure/ --startup-project Api/
```

## Esquema de Base de Datos

Todas las tablas y columnas utilizan nombres en minúsculas para evitar problemas de casing con Npgsql:

- `user` - Usuarios del sistema (incluye `expotoken`)
- `usertoken` - Tokens JWT almacenados
- `event` - Eventos creados por preceptores
- `exitrequest` - Solicitudes de salida de residentes

## Tecnologías

- ASP.NET Core 8.0
- Entity Framework Core
- PostgreSQL
- JWT Authentication
- BCrypt para hashing de contraseñas
- Expo Push Notifications