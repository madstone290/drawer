using Drawer.Application.Services.Organization.Repos;
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
        private readonly ICompanyMemberRepository _companyMemberRepository;

        public UserClaimService(UserManager<IdentityUser> userManager, ICompanyMemberRepository companyMemberRepository)
        {
            _userManager = userManager;
            _companyMemberRepository = companyMemberRepository;
        }

        public async Task<IEnumerable<Claim>> GetClaimsAsync(IdentityUser user)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim(DrawerClaimTypes.UserId, user.Id));
            var member = await _companyMemberRepository.FindByUserIdAsync(user.Id);
            if (member != null)
            {
                claims.Add(new Claim(DrawerClaimTypes.CompanyId, member.CompanyId));
                claims.Add(new Claim(DrawerClaimTypes.IsCompanyOwner, member.IsOwner.ToString()));
            }
            return claims;
        }
    }
}
