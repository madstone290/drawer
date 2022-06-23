using Drawer.Application.Services.Authentication.Repos;
using Drawer.Domain.Models.Authentication;
using Drawer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Infrastructure.Authentication.Repos
{
    public class RefreshTokenRepository : AuthenticationRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(DrawerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IList<RefreshToken>> FindByUserIdAsync(string userId)
        {
            return await _dbContext.RefreshTokens.Where(x => x.UserId == userId).ToListAsync();
        }
    }
}
