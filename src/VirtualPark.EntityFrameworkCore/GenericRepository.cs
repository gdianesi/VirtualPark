using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using VirtualPark.Repository;

namespace VirtualPark.EntityFrameworkCore;

public class GenericRepository<T>(DbContext context) : IRepository<T>
    where T : class
{
    private readonly DbSet<T> _entities = context.Set<T>();
    private readonly DbContext _context = context;

    public void Add(T entity)
    {
        _entities.Add(entity);

        _context.SaveChanges();
    }

    public List<T> GetAll()
    {
        return _entities.ToList();
    }

    public List<T> GetAll(Expression<Func<T, bool>>? predicate)
    {
        IQueryable<T> query = _entities;
        if(predicate != null)
        {
            query = query.Where(predicate);
        }

        return query.ToList();
    }

    public List<T> GetAll(
        Expression<Func<T, bool>>? predicate,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include)
    {
        IQueryable<T> query = _entities;
        if(include != null)
        {
            query = include(query);
        }

        if(predicate != null)
        {
            query = query.Where(predicate);
        }

        return query.ToList();
    }

    public T? Get(Expression<Func<T, bool>> predicate)
    {
        return _entities.FirstOrDefault(predicate);
    }

    public T? Get(
        Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
    {
        IQueryable<T> query = _context.Set<T>();

        if(include != null)
        {
            query = include(query);
        }

        return query.FirstOrDefault(predicate);
    }

    public bool Exist(Expression<Func<T, bool>> predicate)
    {
        return _entities.Any(predicate);
    }

    public void Update(T entity)
    {
        _entities.Update(entity);

        _context.SaveChanges();
    }

    public void Remove(T entity)
    {
        _entities.Remove(entity);

        _context.SaveChanges();
    }
}
