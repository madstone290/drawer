using Drawer.Application.Config;
using Drawer.Application.Services.Authentication.Repos;
using Drawer.Domain.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly UserManager<User> _userManager;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ITokenGenerator _tokenGenerator;


        public RefreshCommandHandler(UserManager<User> userManager, 
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
                throw new AppException(Messages.InvalidLoginInfo);
            if (!user.EmailConfirmed)
                throw new AppException(Messages.InvalidLoginInfo);

            var refreshTokens = await _refreshTokenRepository.FindByUserIdAsync(user.Id);
            var refreshToken = refreshTokens.FirstOrDefault(x => x.Token == command.RefreshToken && x.IsActive);
            if (refreshToken == null)
                throw new AppException(Messages.InvalidRefreshToken);

            // 기간 만료된 토큰 삭제
            var expiredTokens = refreshTokens.Where(x => x.IsExpired).ToList();
            foreach(var token in expiredTokens)
                await _refreshTokenRepository.RemoveAsync(token);

            var claims = await _userManager.GetClaimsAsync(user);
            var accessToken = _tokenGenerator.GenenateAccessToken(claims);

            return new RefreshResult(accessToken);
        }
    }
}
