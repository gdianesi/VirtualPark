namespace VirtualPark.Repository;

public interface IRepository<T> : IReadOnlyRepository<T>
    where T : class
{
    void Add(T entity);

    void Update(T entity);

    void Remove(T entity);
}
