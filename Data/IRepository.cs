using System.Linq.Expressions;

namespace DepDB.Data;

public interface IRepository<T>
{
    Task<T?> GetByIdAsync(string id);
    Task<List<T>> GetAllAsync(int skip = 0, int take = 50);
    Task<List<T>> FindAsync(Expression<Func<T, bool>> filter, int skip = 0, int take = 50);
    Task InsertAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(string id);
}