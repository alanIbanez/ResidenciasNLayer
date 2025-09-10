using Microsoft.EntityFrameworkCore;
using ResidenciasNLayer.Application.Constants;
using ResidenciasNLayer.Domain.Entities;
using ResidenciasNLayer.Infrastructure.Data;

namespace ResidenciasNLayer.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(ResidenciasDbContext context)
    {
        // Seed Roles
        if (!await context.Roles.AnyAsync())
        {
            var roles = new[]
            {
                new Role { Name = RoleNames.Preceptor },
                new Role { Name = RoleNames.Tutor },
                new Role { Name = RoleNames.Guardia },
                new Role { Name = RoleNames.Residente }
            };
            context.Roles.AddRange(roles);
            await context.SaveChangesAsync();
        }

        // Seed PreceptorTypes
        if (!await context.PreceptorTypes.AnyAsync())
        {
            var preceptorTypes = new[]
            {
                new PreceptorType { Name = "Administrador" },
                new PreceptorType { Name = "Monitor" }
            };
            context.PreceptorTypes.AddRange(preceptorTypes);
            await context.SaveChangesAsync();
        }

        // Seed ResidentTypes
        if (!await context.ResidentTypes.AnyAsync())
        {
            var residentTypes = new[]
            {
                new ResidentType { Name = ResidentTypeNames.Universitario },
                new ResidentType { Name = ResidentTypeNames.Colegio }
            };
            context.ResidentTypes.AddRange(residentTypes);
            await context.SaveChangesAsync();
        }

        // Seed ExitTypes
        if (!await context.ExitTypes.AnyAsync())
        {
            var exitTypes = new[]
            {
                new ExitType { Name = ExitTypeNames.Casual },
                new ExitType { Name = ExitTypeNames.Especial }
            };
            context.ExitTypes.AddRange(exitTypes);
            await context.SaveChangesAsync();
        }

        // Seed ExitStatuses
        if (!await context.ExitStatuses.AnyAsync())
        {
            var exitStatuses = new[]
            {
                new ExitStatus { Name = ExitStatusNames.Solicitado },
                new ExitStatus { Name = ExitStatusNames.EnProceso },
                new ExitStatus { Name = ExitStatusNames.AutorizacionTutor },
                new ExitStatus { Name = ExitStatusNames.AutorizacionPreceptor },
                new ExitStatus { Name = ExitStatusNames.Autorizado },
                new ExitStatus { Name = ExitStatusNames.Rechazado },
                new ExitStatus { Name = ExitStatusNames.Cancelado }
            };
            context.ExitStatuses.AddRange(exitStatuses);
            await context.SaveChangesAsync();
        }

        // Seed Shifts
        if (!await context.Shifts.AnyAsync())
        {
            var shifts = new[]
            {
                new Shift { Name = "Mañana", StartTime = new TimeOnly(6, 0), EndTime = new TimeOnly(14, 0) },
                new Shift { Name = "Tarde", StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(22, 0) },
                new Shift { Name = "Noche", StartTime = new TimeOnly(22, 0), EndTime = new TimeOnly(6, 0) }
            };
            context.Shifts.AddRange(shifts);
            await context.SaveChangesAsync();
        }
    }
}