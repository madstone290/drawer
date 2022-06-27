using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Infrastructure.Services.Organization
{
    public interface ICompanyIdProvider
    {
        /// <summary>
        /// 현재 사용자의 회사Id를 가져온다.
        /// </summary>
        /// <returns></returns>
        string? GetCompanyId();
    }
}
