using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Authentication.CommandModels
{
    public class LoginCommandModel 
    {
        public LoginCommandModel() { }
        public LoginCommandModel(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
