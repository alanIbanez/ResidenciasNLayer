using Microsoft.AspNetCore.Mvc;
using ResidenciasNLayer.Entities;
using ResidenciasNLayer.Services.Interfaces;

namespace ResidenciasNLayer.Api.Controllers;

/// <summary>
/// Controller for device operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DevicesController : ControllerBase
{
    private readonly IDeviceService _deviceService;

    public DevicesController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    /// <summary>
    /// Get all devices
    /// </summary>
    /// <returns>List of devices</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Device>>> GetDevices()
    {
        var devices = await _deviceService.GetAllDevicesAsync();
        return Ok(devices);
    }

    /// <summary>
    /// Get device by ID
    /// </summary>
    /// <param name="deviceid">Device ID</param>
    /// <returns>Device</returns>
    [HttpGet("{deviceid}")]
    public async Task<ActionResult<Device>> GetDevice(string deviceid)
    {
        var device = await _deviceService.GetDeviceByIdAsync(deviceid);
        if (device == null)
        {
            return NotFound();
        }
        return Ok(device);
    }

    /// <summary>
    /// Create a new device
    /// </summary>
    /// <param name="device">Device to create</param>
    /// <returns>Created device</returns>
    [HttpPost]
    public async Task<ActionResult<Device>> CreateDevice(Device device)
    {
        if (await _deviceService.DeviceExistsAsync(device.deviceid))
        {
            return Conflict("Device with this ID already exists");
        }

        var createdDevice = await _deviceService.CreateDeviceAsync(device);
        return CreatedAtAction(nameof(GetDevice), new { deviceid = createdDevice.deviceid }, createdDevice);
    }

    /// <summary>
    /// Update an existing device
    /// </summary>
    /// <param name="deviceid">Device ID</param>
    /// <param name="device">Updated device data</param>
    /// <returns>Updated device</returns>
    [HttpPut("{deviceid}")]
    public async Task<ActionResult<Device>> UpdateDevice(string deviceid, Device device)
    {
        if (deviceid != device.deviceid)
        {
            return BadRequest("Device ID mismatch");
        }

        var updatedDevice = await _deviceService.UpdateDeviceAsync(device);
        if (updatedDevice == null)
        {
            return NotFound();
        }

        return Ok(updatedDevice);
    }

    /// <summary>
    /// Delete a device
    /// </summary>
    /// <param name="deviceid">Device ID</param>
    /// <returns>No content if successful</returns>
    [HttpDelete("{deviceid}")]
    public async Task<IActionResult> DeleteDevice(string deviceid)
    {
        var result = await _deviceService.DeleteDeviceAsync(deviceid);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}