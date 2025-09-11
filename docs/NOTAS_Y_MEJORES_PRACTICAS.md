# Notas y Mejores Prácticas - ResidenciasNLayer

## Arquitectura JWT y Autenticación

### Configuración JWT
La aplicación utiliza JSON Web Tokens (JWT) para autenticación. La configuración se encuentra en `appsettings.json`:

```json
{
  "Jwt": {
    "Issuer": "ResidenciasNLayer",
    "Audience": "ResidenciasNLayerClients", 
    "Key": "your-super-secure-jwt-key-with-at-least-32-characters",
    "AccessTokenExpirationMinutes": 60
  }
}
```

**Importante**: En producción, la clave JWT debe ser un secreto seguro y no debe estar en el código fuente.

### FallbackPolicy
Se implementa una política de autorización por defecto que requiere que todos los endpoints estén autenticados:

```csharp
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});
```

Solo los endpoints marcados con `[AllowAnonymous]` (login, register) son accesibles sin autenticación.

### Endpoints de Autenticación
- `POST /api/Auth/register` - Registro de usuarios (permite anónimo)
- `POST /api/Auth/login` - Inicio de sesión (permite anónimo)
- `GET /api/Auth/me` - Información del usuario actual (requiere autenticación)

## Consistencia de Nombres de Roles

### Roles del Sistema
Los roles se definen en `RoleNames`:
- `Preceptor`
- `Tutor`
- `Guardia`
- `Residente`

### Políticas de Autorización
```csharp
- PreceptorOnly: solo Preceptor
- TutorOnly: solo Tutor
- GuardiaOnly: solo Guardia
- ResidenteOnly: solo Residente
- PreceptorOrTutor: Preceptor o Tutor
- PreceptorOrGuardia: Preceptor o Guardia
```

## Hash de Contraseñas con BCrypt

### Implementación
Se usa BCrypt.Net-Next para el hash seguro de contraseñas:

```csharp
// Generar hash
var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

// Verificar contraseña
var isValid = BCrypt.Net.BCrypt.Verify(password, passwordHash);
```

### Mejores Prácticas
- Nunca almacenar contraseñas en texto plano
- BCrypt maneja automáticamente el salt
- BCrypt es resistente a ataques de fuerza bruta

## Integración Expo Push

### Implementación sin Dependencias Externas
Se implementa usando HttpClient directamente a la API de Expo:

```csharp
var httpClient = _httpClientFactory.CreateClient("ExpoNotifications");
var response = await httpClient.PostAsync("--/api/v2/push/send", content);
```

### Seguimiento de Estado
Cada notificación registra:
- `Status`: pending, sent, failed
- `ProviderMessageId`: ID del mensaje de Expo
- `ProviderResponse`: respuesta completa del proveedor

### Resiliencia
- Manejo de errores con try-catch
- Log de todas las operaciones
- Actualización de estado en base de datos
- Continúa funcionando aunque falle el push

## EF Core y Migraciones

### Convención de Nomenclatura PostgreSQL
Todas las tablas usan nombres en minúsculas:
- users
- notification
- exitauthorization
- attendance
- etc.

### Configuración de Entidades
```csharp
builder.ToTable("users"); // Mapeo explícito a minúsculas
```

### Comandos de Migración
```bash
# Instalar herramientas EF
dotnet tool install --global dotnet-ef

# Crear migración
dotnet ef migrations add NombreMigracion

# Aplicar migraciones
dotnet ef database update
```

### Datos Semilla (Seeds)
Los datos de referencia se insertan de forma idempotente:
- Roles del sistema
- Tipos de residente (universitario, colegio)
- Tipos de salida (casual, especial)
- Estados de salida (solicitado, en_proceso, etc.)

## Reglas de Autorización de Salidas

### Flujo de Autorización por Tipo
1. **Casual + Universitario**: Solo requiere Preceptor
2. **Casual + Colegio**: Requiere Tutor + Preceptor
3. **Especial**: Siempre requiere Tutor + Preceptor

### Restricciones de Guardia
- Los guardias NO pueden autorizar salidas
- Los guardias solo registran movimientos físicos cuando la salida ya está `autorizada`
- Acciones de guardia: `guard_departure`, `guard_return`

## Estructura de Base de Datos

### Tabla exit (NO MODIFICAR)
Esta tabla se mantiene intacta según los requisitos. Todas las nuevas funcionalidades se agregan en tablas relacionadas.

### Nuevas Tablas
- `exitauthorization`: registra todas las acciones sobre salidas
- `notification`: notificaciones push con estado
- `attendance`: registro de asistencia (solo inserción)

## Mejores Prácticas de Desarrollo

### Principios SOLID
- Separación clara de responsabilidades por capa
- Inyección de dependencias
- Interfaces bien definidas

### Patrón Repository
- Abstracción de acceso a datos
- Facilita testing con mocks
- Centraliza lógica de persistencia

### Logging
- Usar ILogger para todas las operaciones importantes
- Log de errores con contexto suficiente
- Log de notificaciones enviadas/fallidas

### Testing
- Tests unitarios para políticas de autorización
- Tests de flujos de guardia
- Uso de InMemoryDatabase para tests

### Manejo de Errores
- Excepciones específicas para diferentes escenarios
- Try-catch en controllers para respuestas HTTP apropiadas
- Logging de excepciones

## Configuración de Producción

### Variables de Entorno
Configurar las siguientes variables:
- `JWT_KEY`: Clave secreta para JWT
- `DATABASE_CONNECTION`: Cadena de conexión a PostgreSQL
- Configuración de logging apropiada

### Seguridad
- HTTPS obligatorio en producción
- Configurar CORS apropiadamente
- Validar y sanitizar todas las entradas de usuario

### Performance
- Usar Include() apropiadamente en consultas EF
- Implementar paginación en endpoints que retornan listas
- Configurar connection pooling de base de datos