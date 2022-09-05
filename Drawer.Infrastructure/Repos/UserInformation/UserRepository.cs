using Drawer.Application.Services.UserInformation.QueryModels;
using Drawer.Application.Services.UserInformation.Repos;
using Drawer.Domain.Models.UserInformation;
using Drawer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Infrastructure.Repos.UserInformation
{
    public class UserRepository : Repository<User, long>, IUserRepository
    {
        public UserRepository(DrawerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<User?> FindByIdentityUserId(string identityUserId)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.IdentityUserId == identityUserId);
        }

        public async Task<UserQueryModel?> QueryById(long id)
        {
            return await _dbContext.Users
                .Where(x => x.Id == id)
                .Select(x => new UserQueryModel()
                {
                    Id = x.Id,
                    Email = x.Email,
                    Name = x.Name
                })
                .FirstOrDefaultAsync();
        }

        public async Task<UserQueryModel?> QueryByIdentityUserId(string identityUserId)
        {
            return await _dbContext.Users
                   .Where(x => x.IdentityUserId == identityUserId)
                   .Select(x => new UserQueryModel()
                   {
                       Id = x.Id,
                       Email = x.Email,
                       Name = x.Name
                   })
                   .FirstOrDefaultAsync();
        }
    }
}
