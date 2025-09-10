using Microsoft.EntityFrameworkCore;
using ResidenciasNLayer.Data;
using ResidenciasNLayer.Entities;
using ResidenciasNLayer.Services.Implementations;

namespace ResidenciasNLayer.Api;

/// <summary>
/// Simple test functionality to verify entities and services work correctly
/// </summary>
public static class TestHelper
{
    /// <summary>
    /// Test the entity relationships and service functionality with in-memory database
    /// </summary>
    public static async Task<bool> TestEntitiesAndServicesAsync()
    {
        try
        {
            // Create in-memory database for testing
            var options = new DbContextOptionsBuilder<ResidenciasDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new ResidenciasDbContext(options);
            var deviceService = new DeviceService(context);
            var userService = new UserService(context);

            // Test Device creation
            var device = new Device
            {
                deviceid = "test-device-001",
                tokenfcm = "test-token-12345",
                estado = true
            };

            var createdDevice = await deviceService.CreateDeviceAsync(device);
            if (createdDevice.deviceid != "test-device-001")
                return false;

            // Test User creation with device reference
            var user = new User
            {
                username = "testuser",
                email = "test@example.com",
                deviceid = "test-device-001"
            };

            var createdUser = await userService.CreateUserAsync(user);
            if (createdUser.username != "testuser")
                return false;

            // Test retrieving user by device ID
            var userByDevice = await userService.GetUserByDeviceIdAsync("test-device-001");
            if (userByDevice?.userid != createdUser.userid)
                return false;

            // Test device existence
            var deviceExists = await deviceService.DeviceExistsAsync("test-device-001");
            if (!deviceExists)
                return false;

            Console.WriteLine("✅ All entity and service tests passed!");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Test failed: {ex.Message}");
            return false;
        }
    }
}