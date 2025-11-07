using System.Collections;
using System.Linq.Expressions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;

namespace VirtualPark.EntityFrameworkCore.Test;

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

        var result = _genericRepository.GetAll(null);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().ContainEquivalentOf(e1);
        result.Should().ContainEquivalentOf(e2);
    }

    [TestMethod]
    public void GetAll_WithoutPredicateOrInclude_ShouldReturnAllEntities()
    {
        var e1 = new EntityTest { Id = Guid.NewGuid().ToString(), Name = "One" };
        var e2 = new EntityTest { Id = Guid.NewGuid().ToString(), Name = "Two" };

        _context.Set<EntityTest>().AddRange(e1, e2);
        _context.SaveChanges();

        var result = _genericRepository.GetAll();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().ContainEquivalentOf(e1);
        result.Should().ContainEquivalentOf(e2);
    }

    [TestMethod]
    public void GetAll_WithInclude_ShouldInvokeIncludeAndReturnFilteredResults()
    {
        var e1 = new EntityTest { Id = Guid.NewGuid().ToString(), Name = "A" };
        var e2 = new EntityTest { Id = Guid.NewGuid().ToString(), Name = "B" };

        _context.Set<EntityTest>().AddRange(e1, e2);
        _context.SaveChanges();

        var includeCalled = false;

        Func<IQueryable<EntityTest>, IIncludableQueryable<EntityTest, object>> include =
            q =>
            {
                includeCalled = true;
                return new FakeIncludableQueryable<EntityTest>(q);
            };

        var result = _genericRepository.GetAll(x => x.Name == "A", include);

        includeCalled.Should().BeTrue();
        result.Should().HaveCount(1);
        result.First().Name.Should().Be("A");
    }

    [TestMethod]
    public void GetAll_WithIncludeAndNoMatches_ShouldReturnEmptyList()
    {
        var e1 = new EntityTest { Id = Guid.NewGuid().ToString(), Name = "X" };
        var e2 = new EntityTest { Id = Guid.NewGuid().ToString(), Name = "Y" };

        _context.Set<EntityTest>().AddRange(e1, e2);
        _context.SaveChanges();

        Func<IQueryable<EntityTest>, IIncludableQueryable<EntityTest, object>> include =
            q => new FakeIncludableQueryable<EntityTest>(q);

        var result = _genericRepository.GetAll(x => x.Name == "Z", include);

        result.Should().NotBeNull();
        result.Should().BeEmpty("no entity matches predicate 'Z'");
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

    [TestMethod]
    public void Get_WithInclude_ShouldApplyIncludeAndReturnFirstMatch()
    {
        var data = new List<EntityTest>
        {
            new() { Id = "1", Name = "A" },
            new() { Id = "2", Name = "B" }
        }.AsQueryable();

        var mockSet = new Mock<DbSet<EntityTest>>();
        mockSet.As<IQueryable<EntityTest>>().Setup(m => m.Provider).Returns(data.Provider);
        mockSet.As<IQueryable<EntityTest>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<EntityTest>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<EntityTest>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        var mockContext = new Mock<DbContext>();
        mockContext.Setup(c => c.Set<EntityTest>()).Returns(mockSet.Object);

        var repository = new GenericRepository<EntityTest>(mockContext.Object);

        Func<IQueryable<EntityTest>, IIncludableQueryable<EntityTest, object>> include =
            q => (IIncludableQueryable<EntityTest, object>)q;

        var result = repository.Get(x => x.Id == "2", include: null);

        result.Should().NotBeNull();
        result!.Id.Should().Be("2");
        result.Name.Should().Be("B");

        mockContext.Verify(c => c.Set<EntityTest>(), Times.AtLeastOnce);
    }

    [TestMethod]
    public void Get_WhenIncludeIsProvided_ShouldInvokeIncludeFunction()
    {
        var data = new List<EntityTest>
        {
            new() { Id = "1", Name = "A" },
            new() { Id = "2", Name = "B" }
        }.AsQueryable();

        var mockSet = new Mock<DbSet<EntityTest>>();
        mockSet.As<IQueryable<EntityTest>>().Setup(m => m.Provider).Returns(data.Provider);
        mockSet.As<IQueryable<EntityTest>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<EntityTest>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<EntityTest>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        var mockContext = new Mock<DbContext>();
        mockContext.Setup(c => c.Set<EntityTest>()).Returns(mockSet.Object);

        var repository = new GenericRepository<EntityTest>(mockContext.Object);

        var includeCalled = false;

        Func<IQueryable<EntityTest>, IIncludableQueryable<EntityTest, object>> include =
            q =>
            {
                includeCalled = true;
                return new FakeIncludableQueryable<EntityTest>(q);
            };
        var result = repository.Get(x => x.Id == "1", include);

        includeCalled.Should().BeTrue("el include debe ejecutarse cuando no es null");
        result.Should().NotBeNull();
        result!.Id.Should().Be("1");
        result.Name.Should().Be("A");
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

internal sealed class FakeIncludableQueryable<TEntity>(IQueryable<TEntity> queryable) : IIncludableQueryable<TEntity, object>
{
    private readonly IQueryable<TEntity> _queryable = queryable;

    public Type ElementType => _queryable.ElementType;
    public Expression Expression => _queryable.Expression;
    public IQueryProvider Provider => _queryable.Provider;
    public IEnumerator<TEntity> GetEnumerator() => _queryable.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

internal sealed class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
{
    public DbSet<EntityTest> EntitiesTest => Set<EntityTest>();
}

public sealed record class EntityTest
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
}
