using Drawer.Shared;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Infrastructure.Services.UserInformation
{
    public class UserIdProvider : IUserIdProvider
    {
        /// <summary>
        /// HttpContext가 없는 경우를 식별하기 위한 값
        /// </summary>
        private const string NoHttpContext = "NoHttpContext";
        private readonly string? _userId;

        public UserIdProvider(IHttpContextAccessor accessor)
        {
            if (accessor.HttpContext == null)
            {
                _userId = NoHttpContext;
            }
            else
            {
                _userId = accessor.HttpContext.User
                    .Claims.FirstOrDefault(x => x.Type == DrawerClaimTypes.IdentityUserId)
                    ?.Value;
            }
        }

        public string? GetUserId()
        {
            return _userId;
        }
    }
}
