using Drawer.Application.Config;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.Authentication.Repos;
using Drawer.Application.Services.UserInformation.Repos;
using Drawer.Domain.Models.Authentication;
using Drawer.Domain.Models.UserInformation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Authentication.Commands
{
    public record RegisterCommand(string Email, string Password, string DisplayName) : ICommand<RegisterResult>;

    public record RegisterResult(string UserId, string Email, string DisplayName);

    public class RegisterCommandHandler : ICommandHandler<RegisterCommand, RegisterResult>
    {
        private readonly IAuthenticationUnitOfWork _authenticationUnitOfWork;

        public RegisterCommandHandler(IAuthenticationUnitOfWork authenticationUnitOfWork)
        {
            _authenticationUnitOfWork = authenticationUnitOfWork;
        }

        public async Task<RegisterResult> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            var user = await _authenticationUnitOfWork.UserManager.FindByEmailAsync(command.Email);
            if (user != null)
                throw new DuplicateEmailException();

            user = new IdentityUser()
            {
                UserName = command.Email,
                Email = command.Email,
            };
            var createResult = await _authenticationUnitOfWork.UserManager.CreateAsync(user, command.Password);
            if (!createResult.Succeeded)
                throw new IdentityErrorException(createResult.Errors);

            var userInfo = new UserInfo(user.Id, user.Email, command.DisplayName);
            await _authenticationUnitOfWork.UserInfoRepository.AddAsync(userInfo);
           
            await _authenticationUnitOfWork.SaveChangesAsync();
            return new RegisterResult(userInfo.UserId, userInfo.Email, userInfo.DisplayName);
        }
    }
}
