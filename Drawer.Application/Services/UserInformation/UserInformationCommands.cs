using Drawer.Application.Services.UserInformation.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.UserInformation
{
    public static class UserInformationCommands
    {
        public static UpdateUserCommand UpdateUser(string email, string displayName)
            => new(email, displayName);

        public static UpdatePasswordCommand UpdatePassword(string email, string password, string newPassword)
            => new(email, password, newPassword);
    }
}
