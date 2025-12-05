using UserManagementApi.DTOs;
using UserManagementApi.Entities;

namespace UserManagementApi.Services;

public interface IAuthService
{
    public Task<User> RegisterAsync(RegisterUserDto dto);
    public Task<string> LoginAsync(LoginUserDto dto);
}