using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using VirtualPark.BusinessLogic.UserRoles.Entity;

namespace VirtualPark.DataAccess.Seed;

[ExcludeFromCodeCoverage]
public static class UserRoleSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserRole>().HasData(
            new UserRole
            {
                UserId = Guid.Parse("AAAA9999-1111-1111-1111-111111111111"),
                RoleId = Guid.Parse("AAAA1111-1111-1111-1111-111111111111")
            });
    }
}
