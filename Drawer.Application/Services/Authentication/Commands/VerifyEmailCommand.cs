using Drawer.Application.Config;
using Drawer.Application.Exceptions;
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
    /// <summary>
    /// 메일링크를 검증한다
    /// </summary>
    public class VerifyEmailCommand : ICommand
    {
        public string Email { get; }
        public string Token { get; }

        public VerifyEmailCommand(string email, string token)
        {
            Email = email;
            Token = token;
        }
    }

    public class VerifyEmailCommandHandler : ICommandHandler<VerifyEmailCommand>
    {
        private readonly UserManager<User> _userManager;

        public VerifyEmailCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Unit> Handle(VerifyEmailCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(command.Email);
            if (user is null)
                throw new InvalidEmailException();

            var result = await _userManager.ConfirmEmailAsync(user, command.Token);
            if (!result.Succeeded) 
                throw new IdentityErrorException(result.Errors);

            return Unit.Value;
        }
    }
}
