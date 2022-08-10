using Drawer.Application.Config;
using Drawer.Application.Helpers;
using Drawer.Application.Exceptions;
using Drawer.Domain.Models.Authentication;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drawer.Application.Services.Authentication.CommandModels;

namespace Drawer.Application.Services.Authentication.Commands
{
    /// <summary>
    /// 인증을 위한 메일링크를 송신한다
    /// </summary>
    public record ConfirmEmailCommand(EmailConfirmationCommandModel EmailConfirmation) : ICommand;

    public class ConfirmEmailCommandHandler : ICommandHandler<ConfirmEmailCommand>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ConfirmEmailCommandHandler(UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender; 
        }

        public async Task<Unit> Handle(ConfirmEmailCommand command, CancellationToken cancellationToken)
        {
            var emailConfirmationDto = command.EmailConfirmation;

            var user = await _userManager.FindByEmailAsync(emailConfirmationDto.Email);
            if (user is null)
                throw new InvalidEmailException();

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var linkUri = emailConfirmationDto.RedirectUri
                .AddQuery("token", Uri.EscapeDataString(token))
                .AddQuery("email", user.Email);

            string mailText = $"<a href=\"{linkUri}\">{Messages.ConfirmationEmailText}</a>";

            await _emailSender.SendEmailAsync(emailConfirmationDto.Email, Messages.ConfirmationEmailSubject, mailText, true);
            return Unit.Value;
        }
    }

}
