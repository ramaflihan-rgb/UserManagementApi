using UserManagementApi.DTOs;
using UserManagementApi.Entities;

namespace UserManagementApi.Services;

public interface IUsersService
{
    public Task<List<User>> GetAllUsersAsync();
    public Task<User> GetUserByIdAsync(Guid id);
    public Task<User> UpdateUserAsync(Guid id, UpdateUserDto dto);
    public Task DeleteUserAsync(Guid id);
    public Task ChangePasswordAsync(Guid userId, string oldPassword, string newPassword);

    public Task ChangeUserRoleAsync(Guid id, string roleName);
}