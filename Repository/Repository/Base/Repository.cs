using Domain.Interfaces.Repositories.Base;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository.Base
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext context;

        public Repository(DbContext context)
        {
            this.context = context;
        }

        public async Task<T> AddAsync(T entity)
        {
            var result = await context.AddAsync(entity);
            return result.Entity;
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> expression = null)
        {
            return expression == null ? await context.Set<T>().CountAsync() : await context.Set<T>().Where(expression).CountAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            entity = await context.Set<T>().FindAsync(entity) ?? throw new KeyNotFoundException("Not found");
            context.Entry(entity).State = EntityState.Deleted;
        }

        public async Task<PaginationResult<T>> GetAsync(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string[] include = null, bool isTakeAll = false, bool isDisableTracking = true, int pageIndex = 0, int pageSize = 0)
        {
            IQueryable<T> query = context.Set<T>();
            var paginationResult = new PaginationResult<T>();
            paginationResult.TotalCount = await CountAsync(expression);
            if (expression != null)
                query = query.Where(expression);
            if(isDisableTracking is true)
                query = query.AsNoTracking();
            if (include != null && include.Length > 0)
                foreach (var includeItem in include)
                    query = query.Include(includeItem);
            if (isTakeAll is true)
            {
                if(orderBy != null)
                    paginationResult.Values = await orderBy(query).ToListAsync();
                else
                    paginationResult.Values = await query.ToListAsync();
            }
            else
            {
                paginationResult.PageIndex = pageIndex;
                if (orderBy == null)
                    paginationResult.Values = await query.Skip(pageSize * pageIndex).Take(pageSize).ToListAsync();
                else
                    paginationResult.Values = await orderBy(query).Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            }
            return paginationResult;
        }

        public Task<T> UpdateAsync(T entity)
        {
            if(context.Entry(entity).State == EntityState.Detached)
            {
                context.Attach(entity);
                context.Entry(entity).State = EntityState.Modified;
            }
            return Task.FromResult(entity);
        }
    }
}
