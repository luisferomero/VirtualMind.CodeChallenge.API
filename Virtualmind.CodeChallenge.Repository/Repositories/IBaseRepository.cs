using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Virtualmind.CodeChallenge.Repository.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetListAsync(IList<string> includes);
        Task<TEntity> GetByAsync(Expression<Func<TEntity, bool>> predicate, IList<string> includes);
        Task<List<TEntity>> GetListByAsync(Expression<Func<TEntity, bool>> predicate, IList<string> includes);
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        IQueryable<TEntity> GetQueryable(IList<string> includes);
    }
}
