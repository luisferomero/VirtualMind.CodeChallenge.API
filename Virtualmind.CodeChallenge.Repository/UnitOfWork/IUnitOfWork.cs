using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Virtualmind.CodeChallenge.Repository.Repositories;

namespace Virtualmind.CodeChallenge.Repository.UnitOfWork
{
    public interface IUnitOfWorkService
    {
        IBaseRepository<TEntity> GenericRepository<TEntity>() where TEntity : class;
        bool Complete();
        Task<bool> CompleteAsync();
    }
}
