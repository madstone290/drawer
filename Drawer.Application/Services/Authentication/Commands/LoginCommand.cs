using Drawer.Application.Config;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.Authentication.Repos;
using Drawer.Shared;
using Drawer.Domain.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Drawer.Application.Services.Authentication.CommandModels;

namespace Drawer.Application.Services.Authentication.Commands
{
    public record LoginCommand(LoginCommandModel Login) : ICommand<LoginResponseCommandModel>
    {
        public TimeSpan RefreshTokenLifetime { get; set; } = TimeSpan.FromDays(7);
    }

    public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResponseCommandModel>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserClaimService _userClaimService;

        public LoginCommandHandler(UserManager<IdentityUser> userManager,
            ITokenGenerator tokenGenerator,
            IRefreshTokenRepository refreshTokenRepository,
            IUserClaimService userClaimService)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
            _refreshTokenRepository = refreshTokenRepository;
            _userClaimService = userClaimService;
        }

        public async Task<LoginResponseCommandModel> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var loginDto = command.Login;

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                throw new InvalidLoginException();

            var passwordMatch = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!passwordMatch)
                throw new InvalidLoginException();

            if (!user.EmailConfirmed)
                throw new UnconfirmedEmailException();

            var claims = await _userClaimService.GetClaimsAsync(user);

            var isCompanyMember = claims.Any(x => x.Type == DrawerClaimTypes.CompanyId);
            var isCompanyOwner = claims.Any(x => x.Type == DrawerClaimTypes.IsCompanyOwner && 
                Convert.ToBoolean(x.Value) == true);
            var accessToken = _tokenGenerator.GenenateAccessToken(claims);
            var refreshToken = _tokenGenerator.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken(user.Id, refreshToken, command.RefreshTokenLifetime);
            await _refreshTokenRepository.AddAsync(refreshTokenEntity);
            await _refreshTokenRepository.SaveChangesAsync();

            return new LoginResponseCommandModel()
            {
                IsCompanyMember = isCompanyMember,
                IsCompanyOwner = isCompanyOwner,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }
    }
}
