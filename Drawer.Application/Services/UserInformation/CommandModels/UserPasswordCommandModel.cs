using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.UserInformation.CommandModels
{
    public class UserPasswordCommandModel
    {
        public string Password { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
    }
}
