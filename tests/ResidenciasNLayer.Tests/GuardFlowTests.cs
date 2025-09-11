using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ResidenciasNLayer.Application.Constants;
using ResidenciasNLayer.Application.Interfaces;
using ResidenciasNLayer.Domain.Entities;
using ResidenciasNLayer.Infrastructure.Data;
using ResidenciasNLayer.Infrastructure.Services;
using ResidenciasNLayer.Infrastructure.Repositories;

namespace ResidenciasNLayer.Tests;

public class GuardFlowTests : IDisposable
{
    private readonly ResidenciasDbContext _context;
    private readonly ExitService _exitService;
    private readonly ExitAuthorizationPolicy _authorizationPolicy;

    public GuardFlowTests()
    {
        var options = new DbContextOptionsBuilder<ResidenciasDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ResidenciasDbContext(options);
        _authorizationPolicy = new ExitAuthorizationPolicy(_context);
        
        var exitRepository = new ExitRepository(_context);
        var userRepository = new UserRepository(_context);
        var notificationRepository = new NotificationRepository(_context);
        
        // Create mock notification service that doesn't try to send actual notifications
        var notificationService = new MockNotificationService(notificationRepository);

        _exitService = new ExitService(
            _context,
            exitRepository,
            userRepository,
            _authorizationPolicy,
            notificationService);

        // Seed test data
        SeedTestData();
    }

    [Fact]
    public async Task RecordGuardDepartureAsync_WhenExitIsAuthorized_ShouldSucceed()
    {
        // Arrange
        var exit = await CreateAuthorizedExit();
        var guardUserId = 5; // Guard user

        // Act
        var result = await _exitService.RecordGuardDepartureAsync(exit.Id, guardUserId, DateTime.UtcNow);

        // Assert
        Assert.NotNull(result.ActualDepartureAt);
        
        // Verify authorization was recorded
        var authorization = await _context.ExitAuthorizations
            .FirstOrDefaultAsync(ea => ea.ExitId == exit.Id && ea.Action == ExitAuthorizationActions.GuardDeparture);
        Assert.NotNull(authorization);
        Assert.Equal(guardUserId, authorization.PerformedByUserId);
        Assert.Equal(RoleNames.Guardia, authorization.PerformedByRole);
    }

    [Fact]
    public async Task RecordGuardDepartureAsync_WhenExitIsNotAuthorized_ShouldThrowException()
    {
        // Arrange
        var exit = await CreateUnauthorizedExit();
        var guardUserId = 5; // Guard user

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _exitService.RecordGuardDepartureAsync(exit.Id, guardUserId, DateTime.UtcNow));
    }

    [Fact]
    public async Task RecordGuardReturnAsync_ShouldSucceed()
    {
        // Arrange
        var exit = await CreateAuthorizedExit();
        var guardUserId = 5; // Guard user

        // Act
        var result = await _exitService.RecordGuardReturnAsync(exit.Id, guardUserId, DateTime.UtcNow);

        // Assert
        Assert.NotNull(result.ActualReturnAt);
        
        // Verify authorization was recorded
        var authorization = await _context.ExitAuthorizations
            .FirstOrDefaultAsync(ea => ea.ExitId == exit.Id && ea.Action == ExitAuthorizationActions.GuardReturn);
        Assert.NotNull(authorization);
        Assert.Equal(guardUserId, authorization.PerformedByUserId);
        Assert.Equal(RoleNames.Guardia, authorization.PerformedByRole);
    }

    private async Task<Exit> CreateAuthorizedExit()
    {
        var exit = await CreateTestExit();
        
        // Add required approvals to make it authorized
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
        
        // Update status to authorized
        var autorizadoStatus = await _context.ExitStatuses
            .FirstOrDefaultAsync(es => es.Name == ExitStatusNames.Autorizado);
        exit.ExitStatusId = autorizadoStatus!.Id;
        
        await _context.SaveChangesAsync();
        return exit;
    }

    private async Task<Exit> CreateUnauthorizedExit()
    {
        return await CreateTestExit(); // Just a basic exit without approvals
    }

    private async Task<Exit> CreateTestExit()
    {
        var exit = new Exit
        {
            ResidentId = 1,
            ExitTypeId = 1, // Casual
            ExitStatusId = 1, // Solicitado
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
            new ExitStatus { Id = 1, Name = ExitStatusNames.Solicitado },
            new ExitStatus { Id = 2, Name = ExitStatusNames.Autorizado }
        };

        var shifts = new[]
        {
            new Shift { Id = 1, Name = "Mañana" },
            new Shift { Id = 2, Name = "Tarde" }
        };

        var users = new[]
        {
            new User { Id = 1, Username = "tutor1", PasswordHash = "hash", FullName = "Tutor 1", RoleId = 2, IsActive = true },
            new User { Id = 2, Username = "preceptor1", PasswordHash = "hash", FullName = "Preceptor 1", RoleId = 1, IsActive = true },
            new User { Id = 3, Username = "resident1", PasswordHash = "hash", FullName = "Resident 1", RoleId = 4, IsActive = true },
            new User { Id = 4, Username = "resident2", PasswordHash = "hash", FullName = "Resident 2", RoleId = 4, IsActive = true },
            new User { Id = 5, Username = "guard1", PasswordHash = "hash", FullName = "Guard 1", RoleId = 3, IsActive = true }
        };

        var tutors = new[]
        {
            new Tutor { Id = 1, UserId = 1 }
        };

        var guards = new[]
        {
            new Guard { Id = 1, UserId = 5, ShiftId = 1 }
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
        _context.Shifts.AddRange(shifts);
        _context.Users.AddRange(users);
        _context.Tutors.AddRange(tutors);
        _context.Guards.AddRange(guards);
        _context.Residents.AddRange(residents);
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}

// Mock notification service for testing
public class MockNotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;

    public MockNotificationService(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task NotifyExitRequestedAsync(Exit exit)
    {
        var notification = new Notification
        {
            UserId = exit.Resident.User.Id,
            Type = "exit_requested",
            Title = "Test notification",
            Body = "Test body",
            ExitId = exit.Id,
            Status = "sent",
            SentAt = DateTime.UtcNow
        };
        await _notificationRepository.AddAsync(notification);
    }

    public async Task NotifyExitApprovedAsync(Exit exit, User approver)
    {
        var notification = new Notification
        {
            UserId = exit.Resident.User.Id,
            Type = "exit_approved",
            Title = "Test notification",
            Body = "Test body",
            ExitId = exit.Id,
            Status = "sent",
            SentAt = DateTime.UtcNow
        };
        await _notificationRepository.AddAsync(notification);
    }

    public async Task NotifyExitRejectedAsync(Exit exit, User approver, string reason)
    {
        var notification = new Notification
        {
            UserId = exit.Resident.User.Id,
            Type = "exit_rejected",
            Title = "Test notification",
            Body = "Test body",
            ExitId = exit.Id,
            Status = "sent",
            SentAt = DateTime.UtcNow
        };
        await _notificationRepository.AddAsync(notification);
    }

    public async Task NotifyExitCanceledAsync(Exit exit, User canceledBy, string? reason)
    {
        var notification = new Notification
        {
            UserId = exit.Resident.User.Id,
            Type = "exit_canceled",
            Title = "Test notification",
            Body = "Test body",
            ExitId = exit.Id,
            Status = "sent",
            SentAt = DateTime.UtcNow
        };
        await _notificationRepository.AddAsync(notification);
    }

    public async Task NotifyGuardDepartureAsync(Exit exit, User guard)
    {
        var notification = new Notification
        {
            UserId = exit.Resident.User.Id,
            Type = "guard_departure",
            Title = "Test notification",
            Body = "Test body",
            ExitId = exit.Id,
            Status = "sent",
            SentAt = DateTime.UtcNow
        };
        await _notificationRepository.AddAsync(notification);
    }

    public async Task NotifyGuardReturnAsync(Exit exit, User guard)
    {
        var notification = new Notification
        {
            UserId = exit.Resident.User.Id,
            Type = "guard_return",
            Title = "Test notification",
            Body = "Test body",
            ExitId = exit.Id,
            Status = "sent",
            SentAt = DateTime.UtcNow
        };
        await _notificationRepository.AddAsync(notification);
    }

    public async Task NotifyEventPublishedAsync(int eventId, IEnumerable<User> recipients)
    {
        foreach (var recipient in recipients)
        {
            var notification = new Notification
            {
                UserId = recipient.Id,
                Type = "event_published",
                Title = "Test notification",
                Body = "Test body",
                EventId = eventId,
                Status = "sent",
                SentAt = DateTime.UtcNow
            };
            await _notificationRepository.AddAsync(notification);
        }
    }
}