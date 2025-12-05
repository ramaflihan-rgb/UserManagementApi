using UserManagementApi.Entities;

namespace UserManagementApi.Data;

public class SeedData
{
    public static void Initialize(AppDbContext context)
    {
        
        context.Database.EnsureCreated();

        if (!context.Roles.Any())
        {
            var adminRole = new Role { RoleName = "Admin" };
            var userRole = new Role { RoleName = "User" };
            context.Roles.AddRange(adminRole, userRole);
            context.SaveChanges();
        }


        if (!context.Users.Any(u => u.Email == "admin@example.com"))
        {
            var adminRole = context.Roles.First(r => r.RoleName == "Admin");

            var adminUser = new User(Guid.NewGuid(), "Admin", "admin@example.com",
                BCrypt.Net.BCrypt.HashPassword("Admin123"), adminRole.Id, DateTime.UtcNow);


            context.Users.Add(adminUser);
            context.SaveChanges();
        }
        
    }
}

