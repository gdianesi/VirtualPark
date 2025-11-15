using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace VirtualPark.Repository;

public interface IReadOnlyRepository<T>
    where T : class
{
    T? Get(Expression<Func<T, bool>> predicate);
    T? Get(
        Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
    bool Exist(Expression<Func<T, bool>> expression);

    List<T> GetAll(Expression<Func<T, bool>>? predicate);
    List<T> GetAll(
        Expression<Func<T, bool>>? predicate,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include);
    List<T> GetAll();
}
