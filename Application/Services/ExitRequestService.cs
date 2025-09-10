using ResidenciasNLayer.Application.DTOs;
using ResidenciasNLayer.Application.Interfaces;
using ResidenciasNLayer.Core.Entities;
using ResidenciasNLayer.Core.Interfaces;

namespace ResidenciasNLayer.Application.Services;

public class ExitRequestService : IExitRequestService
{
    private readonly IExitRequestRepository _exitRequestRepository;
    private readonly IResidentRepository _residentRepository;

    public ExitRequestService(IExitRequestRepository exitRequestRepository, IResidentRepository residentRepository)
    {
        _exitRequestRepository = exitRequestRepository;
        _residentRepository = residentRepository;
    }

    public async Task<IEnumerable<ExitRequestDto>> GetExitRequestsByResidentAsync(int residentId)
    {
        var requests = await _exitRequestRepository.GetByResidentIdAsync(residentId);
        return requests.Select(r => new ExitRequestDto
        {
            id = r.id,
            resident_id = r.resident_id,
            type = r.type,
            status = r.status,
            departure_at = r.departure_at,
            return_eta_at = r.return_eta_at,
            authorized_tutor = r.authorized_tutor,
            authorized_preceptor = r.authorized_preceptor,
            guard_exit_at = r.guard_exit_at,
            guard_return_at = r.guard_return_at
        });
    }

    public async Task<ExitRequestDto?> CreateExitRequestAsync(int residentId, CreateExitRequestDto request)
    {
        var resident = await _residentRepository.GetByIdAsync(residentId);
        if (resident == null)
        {
            return null;
        }

        var exitRequest = new exitrequest
        {
            resident_id = residentId,
            type = request.type,
            status = "solicitado",
            departure_at = request.departure_at,
            return_eta_at = request.return_eta_at
        };

        var created = await _exitRequestRepository.AddAsync(exitRequest);
        return new ExitRequestDto
        {
            id = created.id,
            resident_id = created.resident_id,
            type = created.type,
            status = created.status,
            departure_at = created.departure_at,
            return_eta_at = created.return_eta_at,
            authorized_tutor = created.authorized_tutor,
            authorized_preceptor = created.authorized_preceptor,
            guard_exit_at = created.guard_exit_at,
            guard_return_at = created.guard_return_at
        };
    }

    public async Task<bool> AuthorizeExitRequestAsync(int requestId, bool isApproved, bool isTutor)
    {
        var request = await _exitRequestRepository.GetByIdAsync(requestId);
        if (request == null)
        {
            return false;
        }

        if (isTutor)
        {
            request.authorized_tutor = isApproved;
            request.status = isApproved ? "autorizacion_preceptor" : "rechazado";
        }
        else
        {
            request.authorized_preceptor = isApproved;
            request.status = isApproved ? "autorizado" : "rechazado";
        }

        await _exitRequestRepository.UpdateAsync(request);
        return true;
    }

    public async Task<bool> ProcessExitAsync(int requestId)
    {
        var request = await _exitRequestRepository.GetByIdAsync(requestId);
        if (request == null || request.status != "autorizado")
        {
            return false;
        }

        request.guard_exit_at = DateTime.UtcNow;
        request.status = "en_proceso";
        await _exitRequestRepository.UpdateAsync(request);
        return true;
    }

    public async Task<bool> ProcessReturnAsync(int requestId)
    {
        var request = await _exitRequestRepository.GetByIdAsync(requestId);
        if (request == null || request.status != "en_proceso")
        {
            return false;
        }

        request.guard_return_at = DateTime.UtcNow;
        request.status = "completado";
        await _exitRequestRepository.UpdateAsync(request);
        return true;
    }
}