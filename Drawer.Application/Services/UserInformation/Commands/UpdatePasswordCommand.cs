using Drawer.Application.Config;
using Drawer.Application.Exceptions;
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
    public record UpdatePasswordCommand(string UserId, string Password, string NewPassword): ICommand;

    public class UpdatePasswordCommandHandler : ICommandHandler<UpdatePasswordCommand>
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UpdatePasswordCommandHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Unit> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId)
                ?? throw new InvalidUserIdException();

            var result = await _userManager.ChangePasswordAsync(user, request.Password, request.NewPassword);
            if (!result.Succeeded)
                throw new IdentityErrorException(result.Errors);

            return Unit.Value;
        }
    }
}
