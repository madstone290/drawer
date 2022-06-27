using Drawer.Domain.Models.UserInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.UserInformation.Repos
{
    public interface IUserInfoRepository : IRepository<UserInfo>
    {
        Task<UserInfo?> FindByUserIdAsync(string userId);
    }
}
