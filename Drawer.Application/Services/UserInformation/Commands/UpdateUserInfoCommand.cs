using Drawer.Application.Config;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.UserInformation.CommandModels;
using Drawer.Application.Services.UserInformation.Repos;
using Drawer.Domain.Models.Authentication;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.UserInformation.Commands
{
    public record UpdateUserInfoCommand(string UserId, UserInfoCommandModel UserInfo) : ICommand;

    public class UpdateUserCommandHandler : ICommandHandler<UpdateUserInfoCommand>
    {
        private readonly IUserInfoRepository _userInfoRepository;

        public UpdateUserCommandHandler(IUserInfoRepository userInfoRepository)
        {
            _userInfoRepository = userInfoRepository;
        }
        
        public async Task<Unit> Handle(UpdateUserInfoCommand command, CancellationToken cancellationToken)
        {
            var userInfoDto = command.UserInfo;

            var userInfo = await _userInfoRepository.FindByUserIdAsync(command.UserId);
            if (userInfo == null)
                throw new InvalidUserIdException();

            userInfo.SetDisplayName(userInfoDto.DisplayName);
            await _userInfoRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
