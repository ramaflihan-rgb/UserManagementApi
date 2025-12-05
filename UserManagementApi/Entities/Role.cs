namespace UserManagementApi.Entities;

public class Role
{
    public Guid Id { get; set; }
    public string RoleName { get; set; }

    public void  UpdateRole(Guid id, string roleName)
    {
        Id = id;
        RoleName = roleName;
    }
}