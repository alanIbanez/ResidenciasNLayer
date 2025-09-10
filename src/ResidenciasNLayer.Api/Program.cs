using Microsoft.EntityFrameworkCore;
using ResidenciasNLayer.Data;
using ResidenciasNLayer.Services.Interfaces;
using ResidenciasNLayer.Services.Implementations;
using ResidenciasNLayer.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Entity Framework with PostgreSQL
builder.Services.AddDbContext<ResidenciasDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("ResidenciasNLayer.Api")));

// Register services
builder.Services.AddScoped<IDeviceService, DeviceService>();
builder.Services.AddScoped<IUserService, UserService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Run tests to verify entities and services work correctly
Console.WriteLine("🧪 Running entity and service tests...");
var testResult = await TestHelper.TestEntitiesAndServicesAsync();
if (!testResult)
{
    Console.WriteLine("❌ Tests failed! Check the implementation.");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
