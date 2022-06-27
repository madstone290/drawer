using Drawer.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Domain.Models.UserInformation
{
    /// <summary>
    /// Drawer 사용자 정보
    /// </summary>
    public class UserInfo : Entity<long>
    {
        /// <summary>
        /// IdentityUser 아이디
        /// </summary>
        public string UserId { get; private set; }

        /// <summary>
        /// 사용자 이메일
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// 사용자 이름
        /// </summary>
        public string DisplayName { get; private set; } = null!;

        public UserInfo(string userId, string email, string displayName)
        {
            UserId = userId;
            Email = email;
            SetDisplayName(displayName);
        }

        public void SetDisplayName(string displayName)
        {
            if (displayName == null)
                throw new EmptyNameException();
            DisplayName = displayName.Trim();
        }
    }
}
