using Microsoft.EntityFrameworkCore;
using ResidenciasNLayer.Application.Interfaces;
using ResidenciasNLayer.Domain.Entities;
using ResidenciasNLayer.Infrastructure.Data;

namespace ResidenciasNLayer.Application.Services;

public class ExitRequestService : IExitRequestService
{
    private readonly ApplicationDbContext _context;
    private readonly INotificationService _notificationService;

    public ExitRequestService(ApplicationDbContext context, INotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
    }

    public async Task<int> CreateExitRequestAsync(int residentId, int? tutorId, string reason, DateTime requestDate)
    {
        var exitRequest = new ExitRequest
        {
            residentid = residentId,
            tutorid = tutorId,
            reason = reason,
            requestdate = requestDate,
            status = "pending",
            createdat = DateTime.UtcNow,
            updatedat = DateTime.UtcNow
        };

        _context.exitrequests.Add(exitRequest);
        await _context.SaveChangesAsync();

        // Send notifications to preceptors, guards, and the specific tutor
        var notificationTitle = "Nueva solicitud de salida";
        var resident = await _context.users.FindAsync(residentId);
        var notificationBody = $"Solicitud de salida de {resident?.firstname} {resident?.lastname}";

        // Notify all preceptors
        await _notificationService.SendNotificationToRoleAsync("preceptor", notificationTitle, notificationBody);

        // Notify all guards
        await _notificationService.SendNotificationToRoleAsync("guardia", notificationTitle, notificationBody);

        // Notify specific tutor if assigned
        if (tutorId.HasValue)
        {
            await _notificationService.SendNotificationToUsersAsync(new[] { tutorId.Value }, notificationTitle, notificationBody);
        }

        return exitRequest.id;
    }

    public async Task UpdateExitRequestStatusAsync(int requestId, string status, string approvedBy, int approverId)
    {
        var exitRequest = await _context.exitrequests
            .Include(er => er.resident)
            .FirstOrDefaultAsync(er => er.id == requestId);

        if (exitRequest == null)
        {
            throw new InvalidOperationException("Exit request not found");
        }

        exitRequest.status = status;
        exitRequest.approvedby = approvedBy;
        exitRequest.updatedat = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        // Send notification to the resident about the decision
        var notificationTitle = status == "approved" ? "Solicitud aprobada" : "Solicitud rechazada";
        var notificationBody = $"Tu solicitud de salida ha sido {(status == "approved" ? "aprobada" : "rechazada")}";

        await _notificationService.SendNotificationToUsersAsync(
            new[] { exitRequest.residentid }, 
            notificationTitle, 
            notificationBody);
    }

    public async Task ProcessExitAsync(int requestId, int guardId)
    {
        var exitRequest = await _context.exitrequests
            .Include(er => er.resident)
            .Include(er => er.tutor)
            .FirstOrDefaultAsync(er => er.id == requestId);

        if (exitRequest == null)
        {
            throw new InvalidOperationException("Exit request not found");
        }

        exitRequest.status = "exited";
        exitRequest.exitdate = DateTime.UtcNow;
        exitRequest.updatedat = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        // Send notification to tutor and preceptors
        var notificationTitle = "Salida registrada";
        var notificationBody = $"{exitRequest.resident.firstname} {exitRequest.resident.lastname} ha salido de la residencia";

        // Notify tutor
        if (exitRequest.tutorid.HasValue)
        {
            await _notificationService.SendNotificationToUsersAsync(
                new[] { exitRequest.tutorid.Value }, 
                notificationTitle, 
                notificationBody);
        }

        // Notify preceptors
        await _notificationService.SendNotificationToRoleAsync("preceptor", notificationTitle, notificationBody);
    }

    public async Task ProcessReturnAsync(int requestId, int guardId)
    {
        var exitRequest = await _context.exitrequests
            .Include(er => er.resident)
            .Include(er => er.tutor)
            .FirstOrDefaultAsync(er => er.id == requestId);

        if (exitRequest == null)
        {
            throw new InvalidOperationException("Exit request not found");
        }

        exitRequest.status = "returned";
        exitRequest.returndate = DateTime.UtcNow;
        exitRequest.updatedat = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        // Send notification to tutor and preceptors
        var notificationTitle = "Regreso registrado";
        var notificationBody = $"{exitRequest.resident.firstname} {exitRequest.resident.lastname} ha regresado a la residencia";

        // Notify tutor
        if (exitRequest.tutorid.HasValue)
        {
            await _notificationService.SendNotificationToUsersAsync(
                new[] { exitRequest.tutorid.Value }, 
                notificationTitle, 
                notificationBody);
        }

        // Notify preceptors
        await _notificationService.SendNotificationToRoleAsync("preceptor", notificationTitle, notificationBody);
    }

    public async Task<IEnumerable<dynamic>> GetExitRequestsAsync()
    {
        return await _context.exitrequests
            .Include(er => er.resident)
            .Include(er => er.tutor)
            .Select(er => new
            {
                id = er.id,
                resident_name = $"{er.resident.firstname} {er.resident.lastname}",
                tutor_name = er.tutor != null ? $"{er.tutor.firstname} {er.tutor.lastname}" : null,
                reason = er.reason,
                requestdate = er.requestdate,
                exitdate = er.exitdate,
                returndate = er.returndate,
                status = er.status,
                approvedby = er.approvedby,
                createdat = er.createdat
            })
            .ToListAsync();
    }
}