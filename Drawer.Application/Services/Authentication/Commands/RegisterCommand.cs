using Drawer.Application.Config;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.Authentication.CommandModels;
using Drawer.Application.Services.UserInformation.Repos;
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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterCommandHandler(UserManager<IdentityUser> userManager, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            var register = command.Register;

            var identityUser = await _userManager.FindByEmailAsync(register.Email);
            if (identityUser != null)
                throw new DuplicateEmailException();

            identityUser = new IdentityUser()
            {
                UserName = register.Email,
                Email = register.Email,
            };
            var createResult = await _userManager.CreateAsync(identityUser, register.Password);
            if (!createResult.Succeeded)
                throw new IdentityErrorException(createResult.Errors);

            var user = new User(identityUser, identityUser.Email, register.DisplayName);
            await _userRepository.AddAsync(user);
           
            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}
