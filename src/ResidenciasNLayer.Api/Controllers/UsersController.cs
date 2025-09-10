using Microsoft.AspNetCore.Mvc;
using ResidenciasNLayer.Entities;
using ResidenciasNLayer.Services.Interfaces;

namespace ResidenciasNLayer.Api.Controllers;

/// <summary>
/// Controller for user operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns>List of users</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    /// <summary>
    /// Get user by ID
    /// </summary>
    /// <param name="userid">User ID</param>
    /// <returns>User</returns>
    [HttpGet("{userid}")]
    public async Task<ActionResult<User>> GetUser(int userid)
    {
        var user = await _userService.GetUserByIdAsync(userid);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    /// <summary>
    /// Get user by device ID
    /// </summary>
    /// <param name="deviceid">Device ID</param>
    /// <returns>User</returns>
    [HttpGet("by-device/{deviceid}")]
    public async Task<ActionResult<User>> GetUserByDeviceId(string deviceid)
    {
        var user = await _userService.GetUserByDeviceIdAsync(deviceid);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    /// <summary>
    /// Create a new user
    /// </summary>
    /// <param name="user">User to create</param>
    /// <returns>Created user</returns>
    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        var createdUser = await _userService.CreateUserAsync(user);
        return CreatedAtAction(nameof(GetUser), new { userid = createdUser.userid }, createdUser);
    }

    /// <summary>
    /// Update an existing user
    /// </summary>
    /// <param name="userid">User ID</param>
    /// <param name="user">Updated user data</param>
    /// <returns>Updated user</returns>
    [HttpPut("{userid}")]
    public async Task<ActionResult<User>> UpdateUser(int userid, User user)
    {
        if (userid != user.userid)
        {
            return BadRequest("User ID mismatch");
        }

        var updatedUser = await _userService.UpdateUserAsync(user);
        if (updatedUser == null)
        {
            return NotFound();
        }

        return Ok(updatedUser);
    }

    /// <summary>
    /// Delete a user
    /// </summary>
    /// <param name="userid">User ID</param>
    /// <returns>No content if successful</returns>
    [HttpDelete("{userid}")]
    public async Task<IActionResult> DeleteUser(int userid)
    {
        var result = await _userService.DeleteUserAsync(userid);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}