using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagementApi.Entities;

public class User
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public Guid RoleId { get; set; }
    
    [ForeignKey(nameof(RoleId))]
    public Role Role { get; set; }

    public DateTime CreatedAt { get; set; }

    public User(Guid id, string fullName, string email, string passwordHash, Guid roleId, DateTime createdAt)
    {
        Id = id;
        FullName = fullName;
        Email = email;
        PasswordHash = passwordHash;
        RoleId = roleId;
        CreatedAt = createdAt;
    }

    public void UpdateInfo(string fullName, string email)
    {
        FullName = fullName;
        Email = email;
    }
}