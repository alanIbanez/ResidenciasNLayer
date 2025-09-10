using Microsoft.EntityFrameworkCore;
using ResidenciasNLayer.Data;
using ResidenciasNLayer.Entities;
using ResidenciasNLayer.Services.Interfaces;

namespace ResidenciasNLayer.Services.Implementations;

/// <summary>
/// Implementation of device service operations
/// </summary>
public class DeviceService : IDeviceService
{
    private readonly ResidenciasDbContext _context;

    public DeviceService(ResidenciasDbContext context)
    {
        _context = context;
    }

    public async Task<Device?> GetDeviceByIdAsync(string deviceid)
    {
        return await _context.Devices
            .Include(d => d.User)
            .FirstOrDefaultAsync(d => d.deviceid == deviceid);
    }

    public async Task<IEnumerable<Device>> GetAllDevicesAsync()
    {
        return await _context.Devices
            .Include(d => d.User)
            .ToListAsync();
    }

    public async Task<Device> CreateDeviceAsync(Device device)
    {
        _context.Devices.Add(device);
        await _context.SaveChangesAsync();
        return device;
    }

    public async Task<Device?> UpdateDeviceAsync(Device device)
    {
        var existingDevice = await _context.Devices.FindAsync(device.deviceid);
        if (existingDevice == null)
        {
            return null;
        }

        existingDevice.tokenfcm = device.tokenfcm;
        existingDevice.estado = device.estado;

        await _context.SaveChangesAsync();
        return existingDevice;
    }

    public async Task<bool> DeleteDeviceAsync(string deviceid)
    {
        var device = await _context.Devices.FindAsync(deviceid);
        if (device == null)
        {
            return false;
        }

        _context.Devices.Remove(device);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeviceExistsAsync(string deviceid)
    {
        return await _context.Devices.AnyAsync(d => d.deviceid == deviceid);
    }
}