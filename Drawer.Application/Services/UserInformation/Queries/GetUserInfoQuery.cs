using Drawer.Application.Config;
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
    /// <param name="UserId">사용자 아이디</param>
    public record GetUserInfoQuery(string UserId) : IQuery<GetUserInfoResult?>;

    public record GetUserInfoResult(string UserId, string Email, string DisplayName);

    public class GetUserQueryHandler : IQueryHandler<GetUserInfoQuery, GetUserInfoResult?>
    {
        private readonly IUserInfoRepository _userInfoRepository;

        public GetUserQueryHandler(IUserInfoRepository userInfoRepository)
        {
            _userInfoRepository = userInfoRepository;
        }

        public async Task<GetUserInfoResult?> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
        {
            var userInfo = await _userInfoRepository.FindByUserIdAsync(request.UserId);
            if (userInfo == null)
                return null;
            return new GetUserInfoResult(userInfo.UserId, userInfo.Email, userInfo.DisplayName);

        }
    }
}
