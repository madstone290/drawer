﻿using Drawer.Application.Services.Authentication.CommandModels;
using Drawer.Shared;
using Drawer.Shared.Contracts.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Drawer.Web.Authentication
{
    public class AuthenticationManager : IAuthenticationManager
    {
        /// <summary>
        /// Drawer.Api 클라이언트
        /// </summary>
        private readonly HttpClient _httpClient;

        private readonly HttpContext _httpContext;

        private readonly AuthenticationStateProvider _stateProvider;

        public AuthenticationManager(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, AuthenticationStateProvider stateProvider)
        {
            _httpClient = httpClient;
            _httpContext = httpContextAccessor.HttpContext!;
            _stateProvider = stateProvider;
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var loginDto = new LoginCommandModel(email, password);

            var loginResponseMessage = await _httpClient.PostAsJsonAsync(
                ApiRoutes.Account.Login, 
                loginDto);
            if (!loginResponseMessage.IsSuccessStatusCode)
            {
                var error = await loginResponseMessage.Content.ReadFromJsonAsync<ErrorResponse>();
                if (error!.Code == ErrorCodes.UNCONFIRMED_EMAIL)
                {
                    return AuthenticationResult.UnconfirmedEmail();
                }
                else
                {
                    return AuthenticationResult.Fail(error.Message);
                }
            }

            // 쿠키인증을 진행한다.
            var loginResponse = await loginResponseMessage.Content.ReadFromJsonAsync<LoginResponseCommandModel>() ?? default!;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(DrawerClaimTypes.IsCompanyMember, loginResponse.IsCompanyMember.ToString()),
                new Claim(DrawerClaimTypes.IsCompanyOwner, loginResponse.IsCompanyOwner.ToString()),
                new Claim(DrawerClaimTypes.AccessToken, loginResponse.AccessToken),
                new Claim(DrawerClaimTypes.RefreshToken, loginResponse.RefreshToken)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties();

            await _httpContext.SignInAsync(
               CookieAuthenticationDefaults.AuthenticationScheme,
               new ClaimsPrincipal(claimsIdentity),
               authProperties);
            
            return AuthenticationResult.Success();
        }

        public async Task<AuthenticationResult> LogoutAsync()
        {
            await _httpContext.SignOutAsync();
            
            return AuthenticationResult.Success();
        }

        public async Task<UserAuthenticationState> GetUserStateAsync()
        {
            var state = await _stateProvider.GetAuthenticationStateAsync();
            if (state == null)
                return UserAuthenticationState.Unauthenticated();

            if (state.User.Identity == null)
                return UserAuthenticationState.Unauthenticated();

            if (!state.User.Identity.IsAuthenticated)
                return UserAuthenticationState.Unauthenticated();
            
            var emailClaim = state.User.Claims.First(x => x.Type == ClaimTypes.Email);
            var refreshTokenClaim = state.User.Claims.First(x => x.Type == DrawerClaimTypes.RefreshToken);
            return UserAuthenticationState.Authenticated(emailClaim.Value, refreshTokenClaim.Value);
        }

        public async Task<bool> AuthorizeAsync(string permission)
        {
            var state = await _stateProvider.GetAuthenticationStateAsync();
            return state.User.Claims.Any(x => x.Type == permission);
        }

        public async Task<AuthenticationResult> RefreshAsync(bool isCompanyMember, bool isCompanyOwner)
        {
            var claims = _httpContext.User.Claims.ToList();
            
            var isCompanyMemberClaim = claims.First(x => x.Type == DrawerClaimTypes.IsCompanyMember);
            claims.Remove(isCompanyMemberClaim);
            isCompanyMemberClaim = new Claim(DrawerClaimTypes.IsCompanyMember, isCompanyMember.ToString());
            claims.Add(isCompanyMemberClaim);

            var isCompanyOwnerClaim = claims.First(x => x.Type == DrawerClaimTypes.IsCompanyOwner);
            claims.Remove(isCompanyOwnerClaim);
            isCompanyOwnerClaim = new Claim(DrawerClaimTypes.IsCompanyOwner, isCompanyOwner.ToString());
            claims.Add(isCompanyOwnerClaim);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties();

            await _httpContext.SignOutAsync();

            await _httpContext.SignInAsync(
             CookieAuthenticationDefaults.AuthenticationScheme,
             new ClaimsPrincipal(claimsIdentity),
             authProperties);

            return AuthenticationResult.Success();
        }

    }
}
