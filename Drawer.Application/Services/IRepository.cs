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

        Task SaveChangesAsync();
    }
}
