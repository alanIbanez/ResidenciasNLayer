using ResidenciasNLayer.Core.Entities;
using ResidenciasNLayer.Infrastructure.Data;

namespace ResidenciasNLayer.Infrastructure.Data;

public static class SeedData
{
    public static async Task SeedAsync(ResidenciasDbContext context)
    {
        // Seed preceptortypes
        if (!context.preceptortypes.Any())
        {
            context.preceptortypes.AddRange(
                new preceptortype { name = "administrador" },
                new preceptortype { name = "monitor" }
            );
        }

        // Seed residenttypes
        if (!context.residenttypes.Any())
        {
            context.residenttypes.AddRange(
                new residenttype { name = "universitario" },
                new residenttype { name = "colegio" }
            );
        }

        // Seed shifts
        if (!context.shifts.Any())
        {
            context.shifts.AddRange(
                new shift { name = "Mañana", starttime = new TimeOnly(6, 0), endtime = new TimeOnly(14, 0) },
                new shift { name = "Tarde", starttime = new TimeOnly(14, 0), endtime = new TimeOnly(22, 0) },
                new shift { name = "Noche", starttime = new TimeOnly(22, 0), endtime = new TimeOnly(6, 0) }
            );
        }

        await context.SaveChangesAsync();
    }
}