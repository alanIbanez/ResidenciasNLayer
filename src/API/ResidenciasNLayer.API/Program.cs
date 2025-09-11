using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ResidenciasNLayer.Application.Interfaces;
using ResidenciasNLayer.Infrastructure.Data;
using ResidenciasNLayer.Infrastructure.Repositories;
using ResidenciasNLayer.Infrastructure.Services;
using System.Text;
 
var builder = WebApplication.CreateBuilder(args);
 
// Add services to the container.
builder.Services.AddControllers();
 
// Add Entity Framework
builder.Services.AddDbContext<ResidenciasDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
 
// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);
 
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };
});
 
// Configure Authorization with FallbackPolicy (all endpoints require auth by default)
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
        
    // Define role-based policies
    options.AddPolicy("PreceptorOnly", policy => policy.RequireRole("Preceptor"));
    options.AddPolicy("TutorOnly", policy => policy.RequireRole("Tutor"));
    options.AddPolicy("GuardiaOnly", policy => policy.RequireRole("Guardia"));
    options.AddPolicy("ResidenteOnly", policy => policy.RequireRole("Residente"));
    options.AddPolicy("PreceptorOrTutor", policy => policy.RequireRole("Preceptor", "Tutor"));
    options.AddPolicy("PreceptorOrGuardia", policy => policy.RequireRole("Preceptor", "Guardia"));
});
 
// Configure HttpClient for Expo push notifications
builder.Services.AddHttpClient("ExpoNotifications", client =>
{
    client.BaseAddress = new Uri("https://exp.host/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
 
// Register repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IExitRepository, ExitRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
 
// Register services
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IExitAuthorizationPolicy, ExitAuthorizationPolicy>();
builder.Services.AddScoped<IExitService, ExitService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
 
var app = builder.Build();
 
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
 
app.UseHttpsRedirection();
 
app.UseAuthentication();
app.UseAuthorization();
 
app.MapControllers();
 
app.Run();