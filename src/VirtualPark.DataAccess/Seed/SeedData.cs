using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace VirtualPark.DataAccess.Seed;

[ExcludeFromCodeCoverage]
public static class SeedData
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        RoleSeed.Seed(modelBuilder);
        PermissionSeed.Seed(modelBuilder);
        RolePermissionSeed.Seed(modelBuilder);
        UserSeed.Seed(modelBuilder);
        UserRoleSeed.Seed(modelBuilder);
    }
}
