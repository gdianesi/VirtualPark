using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VirtualPark.DataAccess.Test;

[TestClass]
[TestCategory("Repository")]
public class RepositoryTest
{
    private readonly DbContext _context = SqliteInMemoryDbContext.BuildTestDbContext();
    private readonly Repository _repository;

    public RepositoryTest()
    {
        _repository = new Repository<EntityTest>(_context);
    }

    [TestInitialize]
    public void Setup()
    {
        _context.Database.EnsureCreated();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    [TestMethod]
    public void GetAll_Ok()
    {
        var e1 = new EntityTest { Id = Guid.NewGuid().ToString() };
        var e2 = new EntityTest { Id = Guid.NewGuid().ToString() };

        _context.Set<EntityTest>().AddRange(e1, e2);
        _context.SaveChanges();

        var result = _repository.GetAll();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().ContainEquivalentOf(e1);
        result.Should().ContainEquivalentOf(e2);
    }
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
