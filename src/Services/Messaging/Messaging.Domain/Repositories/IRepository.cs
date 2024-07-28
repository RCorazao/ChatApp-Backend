
using Messaging.Domain.Entities.Base;
using System.Linq.Expressions;

namespace Messaging.Domain.Repositories
{
    public interface IRepository<T> where T : IEntity
    {
        Task<T> CreateAsync(T entity);
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter, List<string> excludeFields);
        Task<T> GetAsync(string id);
        Task<T> GetAsync(Expression<Func<T, bool>> filter);
        Task UpdateAsync(T entity);
        Task<bool> RemoveAsync(string id);
    }
}
