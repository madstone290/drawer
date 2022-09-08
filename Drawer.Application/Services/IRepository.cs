using Drawer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        Task AddAsync(TEntity entity);

        void Remove(TEntity entity);
    }

    public interface IRepository<TEntity, TId> : IRepository<TEntity> 
        where TEntity : Entity<TId>
    {
        Task<TEntity?> FindByIdAsync(TId id);

        Task<bool> ExistByIdAsync(TId id);
    }
}
