using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using VirtualPark.BusinessLogic.Roles.Entity;

namespace VirtualPark.DataAccess.Seed;

[ExcludeFromCodeCoverage]
public static class RoleSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role
            {
                Id = Guid.Parse("AAAA1111-1111-1111-1111-111111111111"),
                Name = "Administrator",
                Description = "Full system access"
            },
            new Role
            {
                Id = Guid.Parse("BBBB1111-1111-1111-1111-111111111111"),
                Name = "Operator",
                Description = "Attraction operator"
            },
            new Role
            {
                Id = Guid.Parse("CCCC1111-1111-1111-1111-111111111111"),
                Name = "Visitor",
                Description = "Regular park visitor"
            });
    }
}
