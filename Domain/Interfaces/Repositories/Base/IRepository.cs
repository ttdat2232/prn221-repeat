using Domain.Entities.Base;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories.Base
{
    public interface IRepository<T> where T : class
    {
        Task<PaginationResult<T>> GetAsync(
                Expression<Func<T, bool>> expression = null,
                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                string[] include = null,
                bool isTakeAll = false,
                bool isDisableTracking = true,
                int pageIndex = 0,
                int pageSize = 0
            );
        Task<int> CountAsync(Expression<Func<T, bool>> expression = null);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
