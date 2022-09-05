using Drawer.Application.Services.UserInformation.QueryModels;
using Drawer.Domain.Models.UserInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.UserInformation.Repos
{
    public interface IUserRepository : IRepository<User, long>
    {
        Task<User?> FindByIdentityUserId(string identityUserId);

        Task<UserQueryModel?> QueryById(long id);
        Task<UserQueryModel?> QueryByIdentityUserId(string identityUserId);
    }
}
