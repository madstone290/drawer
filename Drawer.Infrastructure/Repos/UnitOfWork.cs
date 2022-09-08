using Drawer.Application.Services;
using Drawer.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Infrastructure.Repos
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly DrawerDbContext _dbContext;

        public UnitOfWork(DrawerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
