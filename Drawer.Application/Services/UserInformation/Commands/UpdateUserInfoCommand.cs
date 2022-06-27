using Drawer.Application.Config;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.UserInformation.Repos;
using Drawer.Domain.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.UserInformation.Commands
{
    /// <summary>
    /// 사용자 정보를 업데이트한다
    /// </summary>
    /// <param name="UserId">사용자Id</param>
    /// <param name="DisplayName">사용자 이름</param>
    public record UpdateUserInfoCommand(string UserId, string DisplayName) : ICommand<UpdateUserInfoResult>;

    public record UpdateUserInfoResult(string UserId, string DisplayName);

    public class UpdateUserCommandHandler : ICommandHandler<UpdateUserInfoCommand, UpdateUserInfoResult>
    {
        private readonly IUserInfoRepository _userInfoRepository;

        public UpdateUserCommandHandler(IUserInfoRepository userInfoRepository)
        {
            _userInfoRepository = userInfoRepository;
        }
        
        public async Task<UpdateUserInfoResult> Handle(UpdateUserInfoCommand request, CancellationToken cancellationToken)
        {
            var userInfo = await _userInfoRepository.FindByUserIdAsync(request.UserId);
            if (userInfo == null)
                throw new InvalidUserIdException();

            userInfo.SetDisplayName(request.DisplayName);
            await _userInfoRepository.SaveChangesAsync();

            return new UpdateUserInfoResult(userInfo.UserId, userInfo.DisplayName);
        }
    }
}
