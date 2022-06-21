using Drawer.Application.Config;
using Drawer.Application.Exceptions;
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
    /// <param name="Email">사용자 이메일</param>
    /// <param name="DisplayName">사용자 이름</param>
    public record UpdateUserCommand(string Email, string DisplayName) : ICommand<UpdateUserResult>;

    public record UpdateUserResult(string Email, string DisplayName);

    public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, UpdateUserResult>
    {
        private readonly UserManager<User> _userManager;

        public UpdateUserCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<UpdateUserResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new InvalidEmailException();

            user.SetDisplayName(request.DisplayName);
            await _userManager.UpdateAsync(user);

            return new UpdateUserResult(user.Email, user.DisplayName);
        }
    }
}
