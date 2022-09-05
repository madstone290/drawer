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
    public record UpdateUserInfoCommand(string IdentityUserId, UserCommandModel UserInfo) : ICommand;

    public class UpdateUserCommandHandler : ICommandHandler<UpdateUserInfoCommand>
    {
        private readonly IUserRepository _userInfoRepository;

        public UpdateUserCommandHandler(IUserRepository userInfoRepository)
        {
            _userInfoRepository = userInfoRepository;
        }
        
        public async Task<Unit> Handle(UpdateUserInfoCommand command, CancellationToken cancellationToken)
        {
            var userInfoDto = command.UserInfo;

            var user = await _userInfoRepository.FindByIdentityUserId(command.IdentityUserId);
            if (user == null)
                throw new InvalidUserIdException();

            user.SetName(userInfoDto.Name);
            await _userInfoRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
