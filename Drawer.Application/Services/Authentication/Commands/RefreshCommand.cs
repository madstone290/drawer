using Drawer.Application.Config;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.Authentication.CommandModels;
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
    public record RefreshCommand(RefreshCommandModel Refresh) : ICommand<string>;

    public class RefreshCommandHandler : ICommandHandler<RefreshCommand, string>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IUserClaimService _userClaimService;


        public RefreshCommandHandler(UserManager<IdentityUser> userManager,
            ITokenGenerator tokenGenerator,
            IRefreshTokenRepository refreshTokenRepository, 
            IUserClaimService userClaimService)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
            _refreshTokenRepository = refreshTokenRepository;
            _userClaimService = userClaimService;
        }

        public async Task<string> Handle(RefreshCommand command, CancellationToken cancellationToken)
        {
            var refreshDto = command.Refresh;

            var user = await _userManager.FindByEmailAsync(refreshDto.Email);
            if (user == null)
                throw new InvalidLoginException();
            if (!user.EmailConfirmed)
                throw new InvalidLoginException();

            var refreshTokens = await _refreshTokenRepository.FindByUserIdAsync(user.Id);
            var refreshToken = refreshTokens.FirstOrDefault(x => x.Token == refreshDto.RefreshToken && x.IsActive);
            if (refreshToken == null)
                throw new InvalidRefreshTokenException();

            // 기간 만료된 토큰 삭제
            var expiredTokens = refreshTokens.Where(x => x.IsExpired).ToList();
            foreach(var token in expiredTokens)
                _refreshTokenRepository.Remove(token);
            await _refreshTokenRepository.SaveChangesAsync();

            var claims = await _userClaimService.GetClaimsAsync(user);

            var accessToken = _tokenGenerator.GenenateAccessToken(claims);
            return accessToken;
        }
    }
}
