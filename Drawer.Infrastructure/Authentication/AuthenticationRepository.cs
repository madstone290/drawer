﻿using Drawer.Application.Services;
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
        protected readonly DrawerDbContext _dbContext;

        public AuthenticationRepository(DrawerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbContext.AddAsync(entity);
        }

        public async Task<TEntity?> FindByIdAsync(long id)
        {
            return await _dbContext.FindAsync<TEntity>(id);
        }

        public void Remove(TEntity entity)
        {
            _dbContext.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Update(TEntity entity)
        {
            _dbContext.Update(entity);
        }
    }
}
