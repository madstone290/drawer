using Drawer.Application.Services.UserInformation.Repos;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Authentication.Repos
{
    public interface IAuthenticationUnitOfWork : IUnitOfWork
    {
        public UserManager<IdentityUser> UserManager { get; }

        public IUserRepository UserRepository { get; }

    }
}
