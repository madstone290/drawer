using Drawer.Application.Services.Authentication.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Authentication
{
    public static class AuthenticationCommands
    {
        public static RegisterCommand Register(string email, string password, string displayName)
            => new(email, password, displayName);

        public static ConfirmEmailCommand ConfirmEmail(string email, string returnUri)
            => new(email, returnUri);

        public static VerifyEmailCommand VerifyEmail(string email, string token)
            => new(email, token);

        public static LoginCommand Login(string email, string password)
            => new(email, password);

        public static RefreshCommand Refresh(string email, string refreshToken)
            => new(email, refreshToken);
    }
}
