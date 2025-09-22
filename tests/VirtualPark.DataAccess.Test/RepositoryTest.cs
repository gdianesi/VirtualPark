using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VirtualPark.DataAccess.Test;

[TestClass]
[TestCategory("Repository")]
public class RepositoryTest
{
}

internal sealed class TestDbContext : DbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options) { }

    public DbSet<EntityTest> EntitiesTest => Set<EntityTest>();
}

internal sealed record class EntityTest
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
}
