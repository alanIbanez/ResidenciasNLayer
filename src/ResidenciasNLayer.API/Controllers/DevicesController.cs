using Microsoft.AspNetCore.Mvc;
using ResidenciasNLayer.Application.DTOs;
using ResidenciasNLayer.Application.Services;

namespace ResidenciasNLayer.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevicesController : ControllerBase
{
    private readonly IDeviceService _deviceService;

    public DevicesController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    [HttpPost]
    public async Task<ActionResult<DeviceResponseDto>> RegisterDevice([FromBody] DeviceRegistrationDto deviceDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // For now, we'll use a hardcoded user ID. In a real application, 
        // this would come from the authenticated user's claims.
        int userId = 1; // TODO: Get from authenticated user

        try
        {
            var result = await _deviceService.RegisterOrUpdateDeviceAsync(userId, deviceDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while registering the device.", error = ex.Message });
        }
    }

    [HttpGet("{deviceId}")]
    public async Task<ActionResult<DeviceResponseDto>> GetDevice(string deviceId)
    {
        // For now, we'll use a hardcoded user ID. In a real application, 
        // this would come from the authenticated user's claims.
        int userId = 1; // TODO: Get from authenticated user

        try
        {
            var device = await _deviceService.GetDeviceAsync(userId, deviceId);
            
            if (device == null)
            {
                return NotFound(new { message = "Device not found." });
            }

            return Ok(device);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the device.", error = ex.Message });
        }
    }
}