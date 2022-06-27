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
    /// <summary>
    /// 액세스 토큰을 갱신한다
    /// </summary>
    public record RefreshCommand(string Email, string RefreshToken) : ICommand<RefreshResult>;

    /// <summary>
    /// 갱신 결과
    /// </summary>
    /// <param name="AccessToken"></param>
    public record RefreshResult(string AccessToken);

    public class RefreshCommandHandler : ICommandHandler<RefreshCommand, RefreshResult>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ITokenGenerator _tokenGenerator;


        public RefreshCommandHandler(UserManager<IdentityUser> userManager, 
            ITokenGenerator tokenGenerator, 
            IRefreshTokenRepository refreshTokenRepository)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<RefreshResult> Handle(RefreshCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(command.Email);
            if (user == null)
                throw new InvalidLoginException();
            if (!user.EmailConfirmed)
                throw new InvalidLoginException();

            var refreshTokens = await _refreshTokenRepository.FindByUserIdAsync(user.Id);
            var refreshToken = refreshTokens.FirstOrDefault(x => x.Token == command.RefreshToken && x.IsActive);
            if (refreshToken == null)
                throw new InvalidRefreshTokenException();

            // 기간 만료된 토큰 삭제
            var expiredTokens = refreshTokens.Where(x => x.IsExpired).ToList();
            foreach(var token in expiredTokens)
                _refreshTokenRepository.Remove(token);
            await _refreshTokenRepository.SaveChangesAsync();

            var claims = await _userManager.GetClaimsAsync(user);
            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            var accessToken = _tokenGenerator.GenenateAccessToken(claims);
            return new RefreshResult(accessToken);
        }
    }
}
