using Drawer.Application.Services;
using Drawer.Domain.Models;
using Drawer.Infrastructure.Data;
using Drawer.Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Drawer.Infrastructure.Repos
{
    public class Repository<TEntity> : IRepository<TEntity>
       where TEntity : class
    {
        protected readonly DrawerDbContext _dbContext;

        public Repository(DrawerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbContext.AddAsync(entity);
        }

        public void Remove(TEntity entity)
        {
            _dbContext.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
     
    }

    public class Repository<TEntity, TId> : Repository<TEntity>, IRepository<TEntity, TId>
       where TEntity : Entity<TId>
    {
        public Repository(DrawerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> ExistByIdAsync(TId id)
        {
            var lambda = ExpressionUtil.GenericIdEqualExpression<TEntity, TId>(x => x.Id, id);
            return await _dbContext.Set<TEntity>().AnyAsync(lambda);
        }

        public virtual async Task<TEntity?> FindByIdAsync(TId id)
        {
            return await _dbContext.FindAsync<TEntity>(id);
        }
    }
}
