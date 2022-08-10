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
    public class UserInfoRepository : Repository<UserInfo>, IUserInfoRepository
    {
        public UserInfoRepository(DrawerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<UserInfo?> FindByUserIdAsync(string userId)
        {
            return await _dbContext.UserInfos.FirstOrDefaultAsync(x=> x.UserId == userId); 
        }

        public async Task<UserInfoQueryModel?> QueryByUserId(string userId)
        {
            return await _dbContext.UserInfos
                .Where(x => x.UserId == userId)
                .Select(x => new UserInfoQueryModel()
                {
                    UserId = x.UserId,
                    Email = x.Email,
                    DisplayName = x.DisplayName
                })
                .FirstOrDefaultAsync();
        }
    }
}
