using Microsoft.AspNetCore.Mvc;
using TrackerApp.API.Model;
using TrackerApp.API.Services;

namespace TrackerApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllUsers();
        return Ok(users);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetById(int userId)
    {
        var user = await _userService.GetUserByUserId(userId);
        return user is null ? NotFound($"User {userId} not found.") : Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create(User user)
    {
        if (user is null)
            return BadRequest("User cannot be null.");

        var created = await _userService.CreateUser(user);
        return CreatedAtAction(nameof(GetById), new { userId = created.UserId }, created);
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> Update(int userId, User user)
    {
        if (user is null)
            return BadRequest("User cannot be null.");

        var updated = await _userService.UpdateUser(userId, user);
        return updated is null ? NotFound($"User {userId} not found.") : Ok(updated);
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> Delete(int userId)
    {
        var deleted = await _userService.DeleteUser(userId);
        return deleted ? NoContent() : NotFound($"User {userId} not found.");
    }
}