using Domain.Dtos;

namespace Client.Extensions;

public static class RoleDtoExtensions
{
    public static bool IsAdmin(this RoleDto role)
    {
        return role.Name.Equals("Admin", StringComparison.OrdinalIgnoreCase);
    }

    public static bool IsContributor(this RoleDto role)
    {
        return role.Name.Equals("Admin", StringComparison.OrdinalIgnoreCase);
    }
}
