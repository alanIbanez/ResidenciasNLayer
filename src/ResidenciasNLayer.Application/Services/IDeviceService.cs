using ResidenciasNLayer.Application.DTOs;

namespace ResidenciasNLayer.Application.Services;

public interface IDeviceService
{
    Task<DeviceResponseDto> RegisterOrUpdateDeviceAsync(int userId, DeviceRegistrationDto deviceDto);
    Task<DeviceResponseDto?> GetDeviceAsync(int userId, string deviceId);
}