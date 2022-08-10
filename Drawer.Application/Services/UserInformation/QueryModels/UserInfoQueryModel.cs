using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.UserInformation.QueryModels
{
    public class UserInfoQueryModel
    {
        public string UserId { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
    }
}
