using System.ComponentModel.DataAnnotations;

namespace UserManagementApi.DTOs;

public class RegisterUserDto
{
    [Required]
    public string FullName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(8)]
    public string Password { get; set; }

    [Required]
    public string ConfirmPassword { get; set; }
}