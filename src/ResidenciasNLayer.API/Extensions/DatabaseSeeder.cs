using Microsoft.EntityFrameworkCore;
using ResidenciasNLayer.Infrastructure.Data;
using ResidenciasNLayer.Domain.Entities;

namespace ResidenciasNLayer.API.Extensions;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // For in-memory database, just ensure database is created
        // For real database, apply migrations
        if (context.Database.IsInMemory())
        {
            await context.Database.EnsureCreatedAsync();
        }
        else
        {
            await context.Database.MigrateAsync();
        }

        // Check if we already have a test user
        if (!await context.users.AnyAsync())
        {
            // Create a test user
            var testUser = new User
            {
                username = "testuser",
                email = "test@example.com",
                expotoken = "ExponentPushToken[test-token]"
            };

            context.users.Add(testUser);
            await context.SaveChangesAsync();
        }
    }
}