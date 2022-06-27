using Drawer.Application.Services.Authentication.Repos;
using Drawer.Application.Services.UserInformation.Repos;
using Drawer.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Infrastructure.Repos.Authentication
{
    public class AuthenticationUnitOfWork : IAuthenticationUnitOfWork
    {
        private readonly DrawerDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserInfoRepository _userInfoRepository;
        private IDbContextTransaction _transacion;

        public AuthenticationUnitOfWork(DrawerDbContext dbContext,
                                        UserManager<IdentityUser> userManager,
                                        IUserInfoRepository userInfoRepository)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _userInfoRepository = userInfoRepository;
            _transacion = dbContext.Database.BeginTransaction();
        }

        public UserManager<IdentityUser> UserManager => _userManager;
        public IUserInfoRepository UserInfoRepository => _userInfoRepository;

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
            await _transacion.CommitAsync();
        }
    }
}
