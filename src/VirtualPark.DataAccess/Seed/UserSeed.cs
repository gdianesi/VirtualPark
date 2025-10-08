using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using VirtualPark.BusinessLogic.Users.Entity;

namespace VirtualPark.DataAccess.Seed;

[ExcludeFromCodeCoverage]
public static class UserSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = Guid.Parse("AAAA9999-1111-1111-1111-111111111111"),

                Name = "Admin",
                LastName = "System",
                Email = "admin@virtualpark.com",
                Password = "Admin123!",
                VisitorProfileId = null
            });
    }
}
