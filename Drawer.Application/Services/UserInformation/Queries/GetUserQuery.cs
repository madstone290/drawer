using Drawer.Application.Config;
using Drawer.Application.Services.UserInformation.QueryModels;
using Drawer.Application.Services.UserInformation.Repos;
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
    /// <param name="IdentityUserId">IdentityUser 아이디</param>
    public record GetUserQuery(string IdentityUserId) : IQuery<UserQueryModel?>;

    public class GetUserQueryHandler : IQueryHandler<GetUserQuery, UserQueryModel?>
    {
        private readonly IUserRepository _userInfoRepository;

        public GetUserQueryHandler(IUserRepository userInfoRepository)
        {
            _userInfoRepository = userInfoRepository;
        }

        public async Task<UserQueryModel?> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userInfoRepository.QueryByIdentityUserId(request.IdentityUserId);
            return user;

        }
    }
}
