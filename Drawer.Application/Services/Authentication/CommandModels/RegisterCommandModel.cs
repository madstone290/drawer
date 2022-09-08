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
        public RegisterCommandModel(string email, string password, string userName)
        {
            Email = email;
            Password = password;
            UserName = userName;
        }

        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string UserName { get; set; } = default!;
    }
}
