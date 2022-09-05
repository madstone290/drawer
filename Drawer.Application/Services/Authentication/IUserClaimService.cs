using Drawer.Application.Config;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.Organization.Repos;
using Drawer.Application.Services.UserInformation.Repos;
using Drawer.Domain.Models.UserInformation;
using Drawer.Shared;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Authentication
{
    public interface IUserClaimService
    {
        /// <summary>
        /// 사용자의 클레임을 가져온다.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IEnumerable<Claim>> GetClaimsAsync(IdentityUser user);

    }

    public class UserClaimService : IUserClaimService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly ICompanyMemberRepository _companyMemberRepository;

        public UserClaimService(UserManager<IdentityUser> userManager, ICompanyMemberRepository companyMemberRepository, IUserRepository userRepository)
        {
            _userManager = userManager;
            _companyMemberRepository = companyMemberRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<Claim>> GetClaimsAsync(IdentityUser identityUser)
        {
            var claims = await _userManager.GetClaimsAsync(identityUser);
            claims.Add(new Claim(ClaimTypes.Email, identityUser.Email));
            claims.Add(new Claim(DrawerClaimTypes.IdentityUserId, identityUser.Id));

            var user = await _userRepository.FindByIdentityUserId(identityUser.Id)
                ?? throw new AppException("사용자를 찾을 수 없습니다");
            claims.Add(new Claim(DrawerClaimTypes.UserId, $"{user.Id}"));

            var member = await _companyMemberRepository.FindByUserIdAsync(user.Id);
            if (member != null)
            {
                claims.Add(new Claim(DrawerClaimTypes.CompanyId, $"{member.CompanyId}"));
                claims.Add(new Claim(DrawerClaimTypes.IsCompanyOwner, $"{member.IsOwner}"));
            }
            return claims;
        }
    }
}
