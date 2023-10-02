namespace jonas.Application.Interfaces;

public interface IBaseService<T>
{
    Task<T> Get(Guid id);
    Task<IEnumerable<T>> GetAll();
    Task<T> Add(T entity);
    Task<T> Delete(Guid id);
    Task<T> Update(T entity);
}
