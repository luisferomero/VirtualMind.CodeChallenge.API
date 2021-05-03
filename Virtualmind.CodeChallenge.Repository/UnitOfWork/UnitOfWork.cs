using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Virtualmind.CodeChallenge.Repository.Repositories;

namespace Virtualmind.CodeChallenge.Repository.UnitOfWork
{
    public class UnitOfWorkService : IUnitOfWorkService
    {
        private readonly DbContext _dbContext;

        public UnitOfWorkService(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IBaseRepository<TEntity> GenericRepository<TEntity>() where TEntity : class
        {
            return new BaseRepository<TEntity>(_dbContext);
        }

        public bool Complete()
        {
            return _dbContext.SaveChanges() > 0;
        }

        public async Task<bool> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
