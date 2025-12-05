using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UserManagementApi.Data;
using UserManagementApi.DTOs;
using UserManagementApi.Entities;

namespace UserManagementApi.Services;

public class AuthService : IAuthService
{
    
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthService(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<User> RegisterAsync(RegisterUserDto dto)
    {
        if (dto.Password != dto.ConfirmPassword)
            throw new Exception("Passwords do not match");

        if (_context.Users.Any(u => u.Email == dto.Email))
            throw new Exception("Email already exists");

        if (!dto.Password.Any(char.IsUpper) || !dto.Password.Any(char.IsDigit))
            throw new Exception("Password must contain at least one uppercase letter and one number");

        var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "User");
        if (userRole == null) throw new Exception("User role not found");

        var user = new User(Guid.NewGuid(), dto.FullName, dto.Email, BCrypt.Net.BCrypt.HashPassword(dto.Password),
            userRole.Id, DateTime.UtcNow);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<string> LoginAsync(LoginUserDto dto)
    {
        var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new Exception("Invalid email or password");
        
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.RoleName)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}