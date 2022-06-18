using Drawer.Domain.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Authentication
{
    public interface ITokenGenerator
    {
        /// <summary>
        /// 액세스 토큰을 생성한다.
        /// </summary>
        /// <param name="claims">토큰에 포함될 클레임 목록</param>
        /// <returns></returns>
        string GenenateAccessToken(IEnumerable<Claim> claims);

        /// <summary>
        /// 리프레시 토큰을 생성한다.
        /// </summary>
        /// <returns></returns>
        string GenerateRefreshToken();
    }
}
