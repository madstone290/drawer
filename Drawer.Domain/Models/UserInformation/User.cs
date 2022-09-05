using Drawer.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Domain.Models.UserInformation
{
    /// <summary>
    /// 사용자 정보.
    /// IdentityUser와 동일한 ID를 가진다.
    /// </summary>
    public class User : Entity<long>
    {
        public IdentityUser IdentityUser { get; set; } = null!;
        public string IdentityUserId { get; set; } = null!;

        /// <summary>
        /// 사용자 이메일
        /// </summary>
        public string Email { get; private set; } = null!;

        /// <summary>
        /// 사용자 이름
        /// </summary>
        public string Name { get; private set; } = null!;

        private User() { }
        public User(IdentityUser identityUser, string email, string name)
        {
            IdentityUser = identityUser;
            IdentityUserId = identityUser.Id;
            Email = email;
            SetName(name);
        }

        public void SetName(string displayName)
        {
            if (displayName == null)
                throw new EmptyNameException();
            Name = displayName.Trim();
        }
    }
}
