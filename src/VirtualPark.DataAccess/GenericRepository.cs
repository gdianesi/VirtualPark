using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VirtualPark.Repository;

namespace VirtualPark.DataAccess;

public sealed class GenericRepository<T>(DbContext context) : IRepository<T>
    where T : class
{
    private readonly DbSet<T> _entities = context.Set<T>();

    public void Add(T entity)
    {
        throw new NotImplementedException();
    }

    public List<T> GetAll(Expression<Func<T, bool>>? predicate = null)
    {
        if(predicate == null)
        {
            return _entities.ToList();
        }

        return _entities.Where(predicate).ToList();
    }

    public T? Get(Expression<Func<T, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public bool Exist(Expression<Func<T, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public void Update(T entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(T entity)
    {
        throw new NotImplementedException();
    }
}
