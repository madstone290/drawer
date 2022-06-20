using Drawer.Application.Config;
using Drawer.Domain.Models.Authentication;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Authentication.Commands
{
    public class RegisterCommand : ICommand<RegisterResult>
    {
        public string Email { get; }

        public string Password { get; }

        public string DisplayName { get; }

        public RegisterCommand(string email, string password, string displayName)
        {
            Email = email;
            Password = password;
            DisplayName = displayName;
        }
    }

    public class RegisterResult
    {
        public string Id { get; }

        public string Email { get; }

        public string DisplayName { get; }

        public RegisterResult(string id, string email, string displayName)
        {
            Id = id;
            Email = email;
            DisplayName = displayName;
        }
    }

    public class RegisterCommandHandler : ICommandHandler<RegisterCommand, RegisterResult>
    {
        private readonly UserManager<User> _userManager;

        public RegisterCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<RegisterResult> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            User user = await _userManager.FindByEmailAsync(command.Email);
            if (user != null && user.EmailConfirmed)
                throw new AppException(Messages.EmailRegistered);

            // 이메일인증이 되지 않은 경우 재가입
            // TODO 임시 비밀번호를 저장하여 인증이 완료될떄 비밀번호 추가할 것
            if (user != null && !user.EmailConfirmed)
                await _userManager.DeleteAsync(user);

            user = new User(command.Email, command.DisplayName);
            var createResult = await _userManager.CreateAsync(user, command.Password);
            if (!createResult.Succeeded)
            {
                var errorMessage = string.Join(", ", createResult.Errors.Select(x => x.Description));
                throw new AppException(errorMessage);
            }

            return new RegisterResult(user.Id, user.Email, user.DisplayName);
        }
    }
}
