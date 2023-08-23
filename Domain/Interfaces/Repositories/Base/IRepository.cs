using Domain.Models;
using System.Linq.Expressions;

namespace Domain.Interfaces.Repositories.Base
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetById(object id);
        Task<PaginationResult<T>> GetAsync(
                Expression<Func<T, bool>> expression = null,
                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                string[] include = null,
                bool isTakeAll = false,
                bool isDisableTracking = true,
                int pageIndex = 0,
                int pageSize = 4
            );
        Task<int> CountAsync(Expression<Func<T, bool>> expression = null);
        Task<T> AddAsync(T entity);
        T Update(T entity);
        Task DeleteAsync(T entity);
        Task DeleteAsync(object id);
    }
}
