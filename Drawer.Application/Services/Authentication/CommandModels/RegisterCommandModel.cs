using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Authentication.CommandModels
{
    public class RegisterCommandModel
    {
        public RegisterCommandModel() { }
        public RegisterCommandModel(string email, string password, string displayName)
        {
            Email = email;
            Password = password;
            DisplayName = displayName;
        }

        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
    }
}
