using Drawer.Application.Config;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.Authentication.CommandModels;
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
    public record RegisterCommand(RegisterCommandModel Register) : ICommand;

    public class RegisterCommandHandler : ICommandHandler<RegisterCommand>
    {
        private readonly IAuthenticationUnitOfWork _authenticationUnitOfWork;

        public RegisterCommandHandler(IAuthenticationUnitOfWork authenticationUnitOfWork)
        {
            _authenticationUnitOfWork = authenticationUnitOfWork;
        }

        public async Task<Unit> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            var register = command.Register;

            var user = await _authenticationUnitOfWork.UserManager.FindByEmailAsync(register.Email);
            if (user != null)
                throw new DuplicateEmailException();

            user = new IdentityUser()
            {
                UserName = register.Email,
                Email = register.Email,
            };
            var createResult = await _authenticationUnitOfWork.UserManager.CreateAsync(user, register.Password);
            if (!createResult.Succeeded)
                throw new IdentityErrorException(createResult.Errors);

            var userInfo = new UserInfo(user.Id, user.Email, register.DisplayName);
            await _authenticationUnitOfWork.UserInfoRepository.AddAsync(userInfo);
           
            await _authenticationUnitOfWork.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
