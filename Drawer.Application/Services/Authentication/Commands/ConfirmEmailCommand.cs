using Drawer.Application.Config;
using Drawer.Application.Helpers;
using Drawer.Domain.Models.Authentication;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Authentication.Commands
{
    /// <summary>
    /// 인증을 위한 메일링크를 송신한다
    /// </summary>
    public class ConfirmEmailCommand : ICommand
    {
        /// <summary>
        /// 수신 이메일 주소
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// 리디렉트 주소
        /// </summary>
        public string ReturnUri { get; }

        public ConfirmEmailCommand(string email, string returnUri)
        {
            Email = email;
            ReturnUri = returnUri;
        }
    }

    public class ConfirmEmailCommandHandler : ICommandHandler<ConfirmEmailCommand>
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;

        public ConfirmEmailCommandHandler(UserManager<User> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender; 
        }

        public async Task<Unit> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                throw new AppException(Messages.EmailNotRegistered);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var linkUri = request.ReturnUri
                .AddQueryParam("token", Uri.EscapeDataString(token))
                .AddQueryParam("email", user.Email);

            string mailText = $"<a href=\"{linkUri}\">{Messages.ClickLinkToVerify}</a>";

            await _emailSender.SendEmailAsync(request.Email, Messages.ConfirmEmailSubject, mailText, true);
            return Unit.Value;
        }
    }

}
