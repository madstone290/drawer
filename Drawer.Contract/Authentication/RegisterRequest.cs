using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Contract.Authentication
{
    public record RegisterRequest(string Email, string Password, string DisplayName);
}
    