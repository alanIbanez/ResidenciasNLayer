using ResidenciasNLayer.Entities;

namespace ResidenciasNLayer.Services.Interfaces;

/// <summary>
/// Interface for device service operations
/// </summary>
public interface IDeviceService
{
    Task<Device?> GetDeviceByIdAsync(string deviceid);
    Task<IEnumerable<Device>> GetAllDevicesAsync();
    Task<Device> CreateDeviceAsync(Device device);
    Task<Device?> UpdateDeviceAsync(Device device);
    Task<bool> DeleteDeviceAsync(string deviceid);
    Task<bool> DeviceExistsAsync(string deviceid);
}