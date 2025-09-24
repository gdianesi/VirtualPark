using System.Linq.Expressions;

namespace VirtualPark.Repository;

public interface IReadOnlyRepository<T>
    where T : class
{
    T? Get(Expression<Func<T, bool>> predicate);

    bool Exist(Expression<Func<T, bool>> expression);

    List<T> GetAll(Expression<Func<T, bool>>? predicate = null);
}
