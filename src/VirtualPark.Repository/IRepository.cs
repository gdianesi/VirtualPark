using System.Linq.Expressions;

namespace VirtualPark.Repository;

public interface IRepository<T>
    where T : class
{
    void Add(T entity);

    List<T> GetAll(Expression<Func<T, bool>>? predicate = null);

    T? Get(Expression<Func<T, bool>> predicate);

    bool Exist(Expression<Func<T, bool>> expression);

    void Update(T entity);

    void Remove(T entity);
}
