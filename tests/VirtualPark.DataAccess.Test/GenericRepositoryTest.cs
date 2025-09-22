using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace VirtualPark.DataAccess.Test;

[TestClass]
[TestCategory("GenericRepository")]
public class GenericRepositoryTest
{
    private readonly DbContext _context = SqliteInMemoryDbContext.BuildTestDbContext();
    private readonly GenericRepository<EntityTest> _genericRepository;

    public GenericRepositoryTest()
    {
        _genericRepository = new GenericRepository<EntityTest>(_context);
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

    #region GetAll
    #region Success
    [TestMethod]
    public void GetAll_WhenEntitiesExist_ReturnsAll()
    {
        var e1 = new EntityTest { Id = Guid.NewGuid().ToString() };
        var e2 = new EntityTest { Id = Guid.NewGuid().ToString() };

        _context.Set<EntityTest>().AddRange(e1, e2);
        _context.SaveChanges();

        var result = _genericRepository.GetAll();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().ContainEquivalentOf(e1);
        result.Should().ContainEquivalentOf(e2);
    }

    [TestMethod]
    public void GetAll_WithPredicate_ReturnsFilteredById()
    {
        var e1 = new EntityTest { Id = Guid.NewGuid().ToString() };
        var e2 = new EntityTest { Id = Guid.NewGuid().ToString() };

        _context.Set<EntityTest>().AddRange(e1, e2);
        _context.SaveChanges();

        var result = _genericRepository.GetAll(x => x.Id == e1.Id);

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.Should().ContainEquivalentOf(e1);
        result.Should().NotContainEquivalentOf(e2);
    }
    #endregion

    #region Failure
    [TestMethod]
    public void GetAll_WithPredicateNoMatch_ReturnsEmptyList()
    {
        var e1 = new EntityTest { Id = Guid.NewGuid().ToString() };
        var e2 = new EntityTest { Id = Guid.NewGuid().ToString() };
        _context.Set<EntityTest>().AddRange(e1, e2);
        _context.SaveChanges();

        var nonExistentId = Guid.NewGuid().ToString();

        var result = _genericRepository.GetAll(x => x.Id == nonExistentId);

        result.Should().NotBeNull();
        result.Should().BeEmpty("no entity matches the predicate");
    }
    #endregion
    #endregion

    [TestMethod]
    public void Get_OK()
    {
        var e1 = new EntityTest { Id = Guid.NewGuid().ToString() };
        var e2 = new EntityTest { Id = Guid.NewGuid().ToString() };
        _context.Set<EntityTest>().AddRange(e1, e2);
        _context.SaveChanges();

        var result = _genericRepository.Get(x => x.Id == e1.Id);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(e1);
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
