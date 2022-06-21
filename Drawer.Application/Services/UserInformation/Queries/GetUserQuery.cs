using Drawer.Application.Config;
using Drawer.Domain.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.UserInformation.Queries
{
    /// <summary>
    /// 로그인한 사용자 정보를 조회한다.
    /// </summary>
    /// <param name="Email">사용자 이메일</param>
    public record GetUserQuery(string Email) : IQuery<GetUserResult?>;

    public record GetUserResult(string Id, string Email, string DisplayName);

    public class GetUserQueryHandler : IQueryHandler<GetUserQuery, GetUserResult?>
    {
        private readonly UserManager<User> _userManager;

        public GetUserQueryHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<GetUserResult?> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            
            if (user == null)
                return null;
            return new GetUserResult(user.Id, user.Email, user.DisplayName);

        }
    }
}
