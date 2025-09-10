using Microsoft.EntityFrameworkCore;
using ResidenciasNLayer.Application.DTOs;
using ResidenciasNLayer.Application.Services;
using ResidenciasNLayer.Domain.Entities;
using ResidenciasNLayer.Infrastructure.Data;

namespace ResidenciasNLayer.Infrastructure.Services;

public class DeviceService : IDeviceService
{
    private readonly ApplicationDbContext _context;

    public DeviceService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DeviceResponseDto> RegisterOrUpdateDeviceAsync(int userId, DeviceRegistrationDto deviceDto)
    {
        // Check if device already exists for this user
        var existingDevice = await _context.devices
            .FirstOrDefaultAsync(d => d.user_id == userId && d.deviceid == deviceDto.deviceid);

        if (existingDevice != null)
        {
            // Update existing device
            existingDevice.tokenfcm = deviceDto.tokenfcm;
            _context.devices.Update(existingDevice);
            await _context.SaveChangesAsync();

            return new DeviceResponseDto
            {
                id = existingDevice.id,
                deviceid = existingDevice.deviceid,
                tokenfcm = existingDevice.tokenfcm,
                user_id = existingDevice.user_id
            };
        }
        else
        {
            // Create new device
            var newDevice = new Device
            {
                deviceid = deviceDto.deviceid,
                tokenfcm = deviceDto.tokenfcm,
                user_id = userId
            };

            _context.devices.Add(newDevice);
            await _context.SaveChangesAsync();

            return new DeviceResponseDto
            {
                id = newDevice.id,
                deviceid = newDevice.deviceid,
                tokenfcm = newDevice.tokenfcm,
                user_id = newDevice.user_id
            };
        }
    }

    public async Task<DeviceResponseDto?> GetDeviceAsync(int userId, string deviceId)
    {
        var device = await _context.devices
            .FirstOrDefaultAsync(d => d.user_id == userId && d.deviceid == deviceId);

        if (device == null)
            return null;

        return new DeviceResponseDto
        {
            id = device.id,
            deviceid = device.deviceid,
            tokenfcm = device.tokenfcm,
            user_id = device.user_id
        };
    }
}