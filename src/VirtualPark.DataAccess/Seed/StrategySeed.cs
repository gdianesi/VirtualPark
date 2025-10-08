using Microsoft.EntityFrameworkCore;
using VirtualPark.BusinessLogic.Permissions.Entity;

namespace VirtualPark.DataAccess.Seed;

public static class StartegySeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Permission>().HasData(
            new Permission
            {
                Id = Guid.Parse("13131313-1111-1111-1111-111111111111"),
                Key = "CreateActiveStrategy-Strategy",
                Description = "Allows creating new strategy"
            },
            new Permission
            {
                Id = Guid.Parse("13131313-1111-1111-1111-111111111112"),
                Key = "GetActiveStrategy-Strategy",
                Description = "Allows retrieving details of a specific strategy"
            },
            new Permission
            {
                Id = Guid.Parse("13131313-1111-1111-1111-111111111113"),
                Key = "GetActivesStrategies-Strategy",
                Description = "Allows listing all available strategies"
            },
            new Permission
            {
                Id = Guid.Parse("13131313-1111-1111-1111-111111111114"),
                Key = "UpdateStrategy-Strategy",
                Description = "Allows modifying an existing strategy"
            },
            new Permission
            {
                Id = Guid.Parse("13131313-1111-1111-1111-111111111115"),
                Key = "DeleteStrategy-Strategy",
                Description = "Allows deleting an existing strategy"
            });
    }
}
