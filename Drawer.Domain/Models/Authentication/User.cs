using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Domain.Models.Authentication
{
    public class User : IdentityUser
    {
        public string DisplayName { get; set; } = default!;

        public string? RefreshToken { get; set; }

        public User(string email, string displayName)
        {
            Email = email;
            UserName = email;
            DisplayName = displayName;
        }
    }
}
