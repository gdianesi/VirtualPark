using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using VirtualPark.BusinessLogic.TypeIncidences.Entity;

namespace VirtualPark.DataAccess.Seed;

[ExcludeFromCodeCoverage]
public static class TypeIncidenceSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TypeIncidence>().HasData(
            new TypeIncidence
            {
                Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                Type = "PREVENTIVE_MAINTENANCE"
            });
    }
}
