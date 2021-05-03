using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Virtualmind.CodeChallenge.Repository.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _dbContext;

        public BaseRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TEntity>> GetListAsync(IList<string> includes = null)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (includes != null)
                foreach (string incude in includes)
                    query = query.Include(incude);

            return await query.ToListAsync();
        }

        public async Task<TEntity> GetByAsync(Expression<Func<TEntity, bool>> predicate, IList<string> includes = null)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>().Where(predicate);

            if (includes != null)
                foreach (string incude in includes)
                    query = query.Include(incude);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<TEntity>> GetListByAsync(Expression<Func<TEntity, bool>> predicate, IList<string> includes = null)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>().Where(predicate);

            if (includes != null)
                foreach (string incude in includes)
                    query = query.Include(incude);

            return await query.ToListAsync();
        }

        public void Add(TEntity entity)
        {
            _dbContext.AddAsync(entity);
        }

        public void AddRange(IEnumerable<TEntity> entity)
        {
            _dbContext.AddRange(entity);
        }

        public void Update(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(TEntity entity)
        {
            _dbContext.Remove(entity);
        }

        public IQueryable<TEntity> GetQueryable(IList<string> includes = null)
        {
            IQueryable<TEntity> queryable = _dbContext.Set<TEntity>();

            if (includes != null)
                foreach (string incude in includes)
                    queryable = queryable.Include(incude);

            return queryable;
        }
    }
}
