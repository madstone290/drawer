using Drawer.Application.Config;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.Authentication.Repos;
using Drawer.Domain.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Authentication.Commands
{
    public class LoginCommand : ICommand<LoginResult>
    {
        public string Email { get; }
        public string Password { get; }
        public TimeSpan RefreshTokenLifetime { get; } = TimeSpan.FromDays(7);

        public LoginCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public LoginCommand(string email, string password, TimeSpan refreshTokenLifetime) : this(email, password)
        {
            RefreshTokenLifetime = refreshTokenLifetime;
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
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public LoginCommandHandler(UserManager<User> userManager,
            ITokenGenerator tokenGenerator,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<LoginResult> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(command.Email);
            if (user == null)
                throw new InvalidLoginException();

            var passwordMatch = await _userManager.CheckPasswordAsync(user, command.Password);
            if (!passwordMatch)
                throw new InvalidLoginException();

            if (!user.EmailConfirmed)
                throw new UnconfirmedEmailException();

            var claims = await _userManager.GetClaimsAsync(user);
            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            var accessToken = _tokenGenerator.GenenateAccessToken(claims);
            var refreshToken = _tokenGenerator.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken(user.Id, refreshToken, command.RefreshTokenLifetime);
            await _refreshTokenRepository.AddAsync(refreshTokenEntity);
            await _refreshTokenRepository.SaveChangesAsync();

            return new LoginResult(accessToken, refreshToken);
        }
    }
}
