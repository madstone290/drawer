using Drawer.Application.Config;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.Authentication.Repos;
using Drawer.Application.Services.Organization.Repos;
using Drawer.Domain.Models.Authentication;
using Drawer.Shared;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Authentication.Commands
{
    public record LoginCommand(string Email, string Password) : ICommand<LoginResult>
    {
        public TimeSpan RefreshTokenLifetime { get; set; } = TimeSpan.FromDays(7);
    }

    public record LoginResult(string AccessToken, string RefreshToken);

    public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResult>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ICompanyMemberRepository _companyMemberRepository;

        public LoginCommandHandler(UserManager<IdentityUser> userManager,
            ITokenGenerator tokenGenerator,
            IRefreshTokenRepository refreshTokenRepository,
            ICompanyMemberRepository companyMemberRepository)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
            _refreshTokenRepository = refreshTokenRepository;
            _companyMemberRepository = companyMemberRepository;
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
            claims.Add(new Claim(DrawerClaimTypes.UserId, user.Id));
            var member = await _companyMemberRepository.FindByUserIdAsync(user.Id);
            if (member != null)
                claims.Add(new Claim(DrawerClaimTypes.CompanyId, member.CompanyId));

            var accessToken = _tokenGenerator.GenenateAccessToken(claims);
            var refreshToken = _tokenGenerator.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken(user.Id, refreshToken, command.RefreshTokenLifetime);
            await _refreshTokenRepository.AddAsync(refreshTokenEntity);
            await _refreshTokenRepository.SaveChangesAsync();

            return new LoginResult(accessToken, refreshToken);
        }
    }
}
