using Drawer.Application.Config;
using Drawer.Domain.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Authentication.Commands
{
    public class LoginCommand : ICommand<LoginResult>
    {
        public string Email { get; }
        public string Password { get; }

        public LoginCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }

    public class LoginResult
    {
        public string AccessToken { get; }
        public string RefreshToken { get; }

        public LoginResult(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }

    public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenGenerator _tokenGenerator;

        public LoginCommandHandler(UserManager<User> userManager, ITokenGenerator tokenGenerator)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<LoginResult> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(command.Email);
            if (user == null)
                throw new AppException(Messages.InvalidLoginInfo);

            if(!user.EmailConfirmed)
                throw new AppException(Messages.InvalidLoginInfo);

            var passwordMatch = await _userManager.CheckPasswordAsync(user, command.Password);
            if (!passwordMatch)
                throw new AppException(Messages.InvalidLoginInfo);

            return new LoginResult(
                _tokenGenerator.GenenateAccessToken(user),
                _tokenGenerator.GenerateRefreshToken(user));
        }
    }
}
