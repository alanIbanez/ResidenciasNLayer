using Microsoft.EntityFrameworkCore;
using ResidenciasNLayer.Application.Constants;
using ResidenciasNLayer.Domain.Entities;
using ResidenciasNLayer.Infrastructure.Data;
using ResidenciasNLayer.Infrastructure.Services;

namespace ResidenciasNLayer.Tests;

public class ExitAuthorizationPolicyTests : IDisposable
{
    private readonly ResidenciasDbContext _context;
    private readonly ExitAuthorizationPolicy _policy;

    public ExitAuthorizationPolicyTests()
    {
        var options = new DbContextOptionsBuilder<ResidenciasDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ResidenciasDbContext(options);
        _policy = new ExitAuthorizationPolicy(_context);

        // Seed test data
        SeedTestData();
    }

    [Fact]
    public async Task GetRequiredAsync_CasualExitWithUniversitarioResident_RequiresOnlyPreceptor()
    {
        // Arrange
        var exit = await CreateTestExit(ExitTypeNames.Casual, ResidentTypeNames.Universitario);

        // Act
        var (tutorRequired, preceptorRequired) = await _policy.GetRequiredAsync(exit);

        // Assert
        Assert.False(tutorRequired);
        Assert.True(preceptorRequired);
    }

    [Fact]
    public async Task GetRequiredAsync_CasualExitWithColegioResident_RequiresBothTutorAndPreceptor()
    {
        // Arrange
        var exit = await CreateTestExit(ExitTypeNames.Casual, ResidentTypeNames.Colegio);

        // Act
        var (tutorRequired, preceptorRequired) = await _policy.GetRequiredAsync(exit);

        // Assert
        Assert.True(tutorRequired);
        Assert.True(preceptorRequired);
    }

    [Fact]
    public async Task GetRequiredAsync_EspecialExit_RequiresBothTutorAndPreceptor()
    {
        // Arrange
        var exit = await CreateTestExit(ExitTypeNames.Especial, ResidentTypeNames.Universitario);

        // Act
        var (tutorRequired, preceptorRequired) = await _policy.GetRequiredAsync(exit);

        // Assert
        Assert.True(tutorRequired);
        Assert.True(preceptorRequired);
    }

    [Fact]
    public async Task IsFullyAuthorizedAsync_WithRequiredApprovals_ReturnsTrue()
    {
        // Arrange
        var exit = await CreateTestExit(ExitTypeNames.Especial, ResidentTypeNames.Universitario);
        
        // Add required authorizations
        var tutorAuth = new ExitAuthorization
        {
            ExitId = exit.Id,
            PerformedByUserId = 1,
            PerformedByRole = RoleNames.Tutor,
            Action = ExitAuthorizationActions.Approved,
            PerformedAt = DateTime.UtcNow
        };
        
        var preceptorAuth = new ExitAuthorization
        {
            ExitId = exit.Id,
            PerformedByUserId = 2,
            PerformedByRole = RoleNames.Preceptor,
            Action = ExitAuthorizationActions.Approved,
            PerformedAt = DateTime.UtcNow
        };

        _context.ExitAuthorizations.AddRange(tutorAuth, preceptorAuth);
        await _context.SaveChangesAsync();

        // Act
        var isAuthorized = await _policy.IsFullyAuthorizedAsync(exit);

        // Assert
        Assert.True(isAuthorized);
    }

    [Fact]
    public async Task IsFullyAuthorizedAsync_WithMissingApprovals_ReturnsFalse()
    {
        // Arrange
        var exit = await CreateTestExit(ExitTypeNames.Especial, ResidentTypeNames.Universitario);
        
        // Add only tutor authorization (missing preceptor)
        var tutorAuth = new ExitAuthorization
        {
            ExitId = exit.Id,
            PerformedByUserId = 1,
            PerformedByRole = RoleNames.Tutor,
            Action = ExitAuthorizationActions.Approved,
            PerformedAt = DateTime.UtcNow
        };

        _context.ExitAuthorizations.Add(tutorAuth);
        await _context.SaveChangesAsync();

        // Act
        var isAuthorized = await _policy.IsFullyAuthorizedAsync(exit);

        // Assert
        Assert.False(isAuthorized);
    }

    private async Task<Exit> CreateTestExit(string exitTypeName, string residentTypeName)
    {
        var exitType = await _context.ExitTypes.FirstOrDefaultAsync(et => et.Name == exitTypeName);
        var residentType = await _context.ResidentTypes.FirstOrDefaultAsync(rt => rt.Name == residentTypeName);
        var resident = await _context.Residents.FirstOrDefaultAsync(r => r.ResidentType.Name == residentTypeName);

        var exit = new Exit
        {
            ResidentId = resident!.Id,
            ExitTypeId = exitType!.Id,
            ExitStatusId = 1,
            PlannedDepartureAt = DateTime.UtcNow.AddHours(2),
            PlannedReturnAt = DateTime.UtcNow.AddHours(6),
            RequestedAt = DateTime.UtcNow
        };

        _context.Exits.Add(exit);
        await _context.SaveChangesAsync();

        // Reload with includes
        return await _context.Exits
            .Include(e => e.Resident)
                .ThenInclude(r => r.ResidentType)
            .Include(e => e.ExitType)
            .FirstOrDefaultAsync(e => e.Id == exit.Id) ?? exit;
    }

    private void SeedTestData()
    {
        // Seed lookup data
        var roles = new[]
        {
            new Role { Id = 1, Name = RoleNames.Preceptor },
            new Role { Id = 2, Name = RoleNames.Tutor },
            new Role { Id = 3, Name = RoleNames.Guardia },
            new Role { Id = 4, Name = RoleNames.Residente }
        };

        var exitTypes = new[]
        {
            new ExitType { Id = 1, Name = ExitTypeNames.Casual },
            new ExitType { Id = 2, Name = ExitTypeNames.Especial }
        };

        var residentTypes = new[]
        {
            new ResidentType { Id = 1, Name = ResidentTypeNames.Universitario },
            new ResidentType { Id = 2, Name = ResidentTypeNames.Colegio }
        };

        var exitStatuses = new[]
        {
            new ExitStatus { Id = 1, Name = ExitStatusNames.Solicitado }
        };

        var users = new[]
        {
            new User { Id = 1, Username = "tutor1", PasswordHash = "hash", FullName = "Tutor 1", RoleId = 2, IsActive = true },
            new User { Id = 2, Username = "preceptor1", PasswordHash = "hash", FullName = "Preceptor 1", RoleId = 1, IsActive = true },
            new User { Id = 3, Username = "resident1", PasswordHash = "hash", FullName = "Resident 1", RoleId = 4, IsActive = true },
            new User { Id = 4, Username = "resident2", PasswordHash = "hash", FullName = "Resident 2", RoleId = 4, IsActive = true }
        };

        var tutors = new[]
        {
            new Tutor { Id = 1, UserId = 1 }
        };

        var residents = new[]
        {
            new Resident { Id = 1, UserId = 3, ResidentTypeId = 1, TutorId = 1 }, // Universitario
            new Resident { Id = 2, UserId = 4, ResidentTypeId = 2, TutorId = 1 }  // Colegio
        };

        _context.Roles.AddRange(roles);
        _context.ExitTypes.AddRange(exitTypes);
        _context.ResidentTypes.AddRange(residentTypes);
        _context.ExitStatuses.AddRange(exitStatuses);
        _context.Users.AddRange(users);
        _context.Tutors.AddRange(tutors);
        _context.Residents.AddRange(residents);
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}