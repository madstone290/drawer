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
        public static RegisterCommand RegisterCommand(string email, string password, string displayName)
            => new(email, password, displayName);
    }
}
