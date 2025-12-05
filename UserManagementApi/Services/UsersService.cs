using Microsoft.EntityFrameworkCore;
using UserManagementApi.Data;
using UserManagementApi.DTOs;
using UserManagementApi.Entities;

namespace UserManagementApi.Services;

public class UsersService : IUsersService
{
    private readonly AppDbContext _context;

    public UsersService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users.Include(u => u.Role).ToListAsync();
    }

    public async Task<User> GetUserByIdAsync(Guid id)
    {
        var userId = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == id);
        
        if (userId == null)
        {
            throw new Exception("User not found");
        }

        return userId;
    }

    public async Task<User> UpdateUserAsync(Guid id, UpdateUserDto dto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) throw new Exception("User not found");
        
        var emailExists = await _context.Users
            .AnyAsync(u => u.Email == dto.Email && u.Id != id);
        if (emailExists)
            throw new Exception("Email is already in use by another user");
        
        user.UpdateInfo(dto.FullName, dto.Email);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task DeleteUserAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) throw new Exception("User not found");

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task ChangePasswordAsync(Guid userId, string oldPassword, string newPassword)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) throw new Exception("User not found");

        if (!BCrypt.Net.BCrypt.Verify(oldPassword, user.PasswordHash))
            throw new Exception("Old password is incorrect");

        if (newPassword.Length < 8 || !newPassword.Any(char.IsUpper) || !newPassword.Any(char.IsDigit))
            throw new Exception("Password must be at least 8 chars, include an uppercase letter and a number");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        await _context.SaveChangesAsync();
    }

    public async Task ChangeUserRoleAsync(Guid id, string roleName)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) throw new Exception("User not found");

        var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
        if (role == null) throw new Exception("Role not found");

        user.RoleId = role.Id;
        user.Role = role;
        role.UpdateRole(role.Id, roleName);
        await _context.SaveChangesAsync();
    }
}