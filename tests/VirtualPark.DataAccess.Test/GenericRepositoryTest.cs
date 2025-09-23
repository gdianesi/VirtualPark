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

    #region Get
    #region Success
    [TestMethod]
    public void Get_WithValidPredicate_ReturnsEntity()
    {
        var e1 = new EntityTest { Id = Guid.NewGuid().ToString() };
        var e2 = new EntityTest { Id = Guid.NewGuid().ToString() };
        _context.Set<EntityTest>().AddRange(e1, e2);
        _context.SaveChanges();

        var result = _genericRepository.Get(x => x.Id == e1.Id);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(e1);
    }
    #endregion

    #region Failure
    [TestMethod]
    public void Get_WithInvalidPredicate_ReturnsNull()
    {
        var e1 = new EntityTest { Id = Guid.NewGuid().ToString() };
        var e2 = new EntityTest { Id = Guid.NewGuid().ToString() };
        _context.Set<EntityTest>().AddRange(e1, e2);
        _context.SaveChanges();

        var nonExistentId = Guid.NewGuid().ToString();

        var result = _genericRepository.Get(x => x.Id == nonExistentId);

        result.Should().BeNull("no entity should match the given predicate");
    }
    #endregion
    #endregion

    #region Add
    #region Success
    [TestMethod]
    public void Add_NewEntity_PersistsInDatabase()
    {
        var entity = new EntityTest { Id = Guid.NewGuid().ToString() };

        _genericRepository.Add(entity);
        _context.SaveChanges();

        var result = _context.Set<EntityTest>().Find(entity.Id);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(entity);
    }
    #endregion
    #endregion

    #region Exist
    #region Success
    [TestMethod]
    public void Exist_WithMatchingPredicate__ReturnsTrue()
    {
        var e1 = new EntityTest { Id = Guid.NewGuid().ToString() };
        var e2 = new EntityTest { Id = Guid.NewGuid().ToString() };

        _context.Set<EntityTest>().AddRange(e1, e2);
        _context.SaveChanges();

        var result = _genericRepository.Exist(x => x.Id == e1.Id);

        result.Should().BeTrue();
    }
    #endregion

    #region Failure
    [TestMethod]
    public void Exist_WithNonMatchingPredicate_ReturnsFalse()
    {
        var e1 = new EntityTest { Id = Guid.NewGuid().ToString() };
        var e2 = new EntityTest { Id = Guid.NewGuid().ToString() };

        _context.Set<EntityTest>().AddRange(e1, e2);
        _context.SaveChanges();

        var nonExistentId = Guid.NewGuid().ToString();

        var result = _genericRepository.Exist(x => x.Id == nonExistentId);

        result.Should().BeFalse();
    }
    #endregion
    #endregion

    #region Update
    #region Success
    [TestMethod]
    public void Update_ExistingEntity_UpdatesSuccessfully()
    {
        var entity = new EntityTest { Id = Guid.NewGuid().ToString(), Name = "Old" };
        _context.Set<EntityTest>().Add(entity);
        _context.SaveChanges();

        entity.Name = "New";
        _genericRepository.Update(entity);

        var result = _context.Set<EntityTest>().Find(entity.Id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(entity.Id);
        result.Name.Should().Be("New");
    }
    #endregion
    #endregion

    #region Delete
    #region Success
    [TestMethod]
    public void Remove_ExistingEntity_DeletesSuccessfully()
    {
        var entity = new EntityTest { Id = Guid.NewGuid().ToString() };
        _context.Set<EntityTest>().Add(entity);
        _context.SaveChanges();

        _genericRepository.Remove(entity);

        var result = _context.Set<EntityTest>().Find(entity.Id);

        result.Should().BeNull();
    }
    #endregion
    #endregion
}

internal sealed class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
{
    public DbSet<EntityTest> EntitiesTest => Set<EntityTest>();
}

internal sealed record class EntityTest
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
}
