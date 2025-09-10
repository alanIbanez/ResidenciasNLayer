using Microsoft.EntityFrameworkCore;
using ResidenciasNLayer.Application.Constants;
using ResidenciasNLayer.Application.Interfaces;
using ResidenciasNLayer.Domain.Entities;
using ResidenciasNLayer.Infrastructure.Data;

namespace ResidenciasNLayer.Infrastructure.Services;

public class ExitAuthorizationPolicy : IExitAuthorizationPolicy
{
    private readonly ResidenciasDbContext _context;

    public ExitAuthorizationPolicy(ResidenciasDbContext context)
    {
        _context = context;
    }

    public async Task<(bool tutorRequired, bool preceptorRequired)> GetRequiredAsync(Exit exit)
    {
        var resident = await _context.Residents
            .Include(r => r.ResidentType)
            .FirstOrDefaultAsync(r => r.Id == exit.ResidentId);

        var exitType = await _context.ExitTypes
            .FirstOrDefaultAsync(et => et.Id == exit.ExitTypeId);

        if (resident == null || exitType == null)
            throw new InvalidOperationException("Invalid exit data");

        // Authorization rules:
        // - Casual + ResidentType=universitario: requires only Preceptor approval
        // - Casual + ResidentType=colegio: requires Tutor + Preceptor
        // - Especial: always requires Tutor + Preceptor

        if (exitType.Name == ExitTypeNames.Especial)
        {
            return (tutorRequired: true, preceptorRequired: true);
        }
        
        if (exitType.Name == ExitTypeNames.Casual)
        {
            if (resident.ResidentType.Name == ResidentTypeNames.Universitario)
            {
                return (tutorRequired: false, preceptorRequired: true);
            }
            else if (resident.ResidentType.Name == ResidentTypeNames.Colegio)
            {
                return (tutorRequired: true, preceptorRequired: true);
            }
        }

        throw new InvalidOperationException($"Unknown exit type: {exitType.Name} or resident type: {resident.ResidentType.Name}");
    }

    public async Task<bool> IsFullyAuthorizedAsync(Exit exit)
    {
        var (tutorRequired, preceptorRequired) = await GetRequiredAsync(exit);
        
        var authorizations = await _context.ExitAuthorizations
            .Where(ea => ea.ExitId == exit.Id && ea.Action == ExitAuthorizationActions.Approved)
            .ToListAsync();

        bool tutorApproved = !tutorRequired || authorizations.Any(a => a.PerformedByRole == RoleNames.Tutor);
        bool preceptorApproved = !preceptorRequired || authorizations.Any(a => a.PerformedByRole == RoleNames.Preceptor);

        return tutorApproved && preceptorApproved;
    }

    public async Task<bool> CanUserApproveAsync(int userId, Exit exit)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) return false;

        var (tutorRequired, preceptorRequired) = await GetRequiredAsync(exit);

        // Check if this user role is required for approval
        if (user.Role.Name == RoleNames.Tutor && tutorRequired)
        {
            // Check if tutor is assigned to this resident
            var resident = await _context.Residents
                .Include(r => r.Tutor)
                .FirstOrDefaultAsync(r => r.Id == exit.ResidentId);
            
            return resident?.Tutor?.UserId == userId;
        }

        if (user.Role.Name == RoleNames.Preceptor && preceptorRequired)
        {
            return true; // Any preceptor can approve
        }

        return false;
    }
}