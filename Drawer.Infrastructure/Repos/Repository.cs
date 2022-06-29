using Drawer.Application.Services;
using Drawer.Domain.Models;
using Drawer.Infrastructure.Data;


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

        public async Task<TEntity?> FindByIdAsync(TId id)
        {
            return await _dbContext.FindAsync<TEntity>(id);
        }
    }
}
