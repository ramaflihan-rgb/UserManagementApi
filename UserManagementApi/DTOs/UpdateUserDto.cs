using System.ComponentModel.DataAnnotations;

namespace UserManagementApi.DTOs;

public class UpdateUserDto
{
    [Required]
    public string FullName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }
}