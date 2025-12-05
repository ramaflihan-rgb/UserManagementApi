using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagementApi.DTOs;
using UserManagementApi.Services;

namespace UserManagementApi.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize] 
public class UsersController : ControllerBase
{
    private readonly IUsersService _usersService;

    public UsersController(IUsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUsers()
    {
        return Ok(await _usersService.GetAllUsersAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        return Ok(await _usersService.GetUserByIdAsync(id));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDto dto) =>
        Ok(await _usersService.UpdateUserAsync(id, dto));

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        await _usersService.DeleteUserAsync(id);
        return Ok("User deleted successfully");
    }

    [HttpPut("{id}/role")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeRole(Guid id, [FromBody] string roleName)
    {
        await _usersService.ChangeUserRoleAsync(id, roleName);
        return Ok($"Role changed to {roleName}");
    }
    
    [HttpPut("me/password")]
    [Authorize] 
    public async Task<IActionResult> ChangeMyPassword([FromBody] ChangePasswordDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized();

        var userId = Guid.Parse(userIdClaim);

        await _usersService.ChangePasswordAsync(userId, dto.OldPassword, dto.NewPassword);
        return Ok("Password changed successfully");
    }


}