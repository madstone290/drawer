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
        public string DisplayName { get; private set; }

        public User(string email, string displayName)
        {
            Email = email.Trim();
            UserName = email.Trim();
            DisplayName = displayName.Trim();
        }

        /// <summary>
        /// 이름을 변경한다.
        /// </summary>
        /// <param name="displayName"></param>
        public void SetDisplayName(string displayName)
        {
            DisplayName = displayName.Trim();
        }

    }
}
