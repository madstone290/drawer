using Drawer.Application.Services;
using Drawer.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Infrastructure.Authentication
{
    public class AuthenticationRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected readonly DrawerIdentityDbContext _dbContext;

        public AuthenticationRepository(DrawerIdentityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<TEntity?> FindByIdAsync(long id)
        {
            return await _dbContext.FindAsync<TEntity>(id);
        }

        public async Task RemoveAsync(TEntity entity)
        {
            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
