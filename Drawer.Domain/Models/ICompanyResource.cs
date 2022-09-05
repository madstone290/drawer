using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Domain.Models
{
    /// <summary>
    /// 회사 자원.
    /// 회사에 속한 구성원들만 접근가능하다.
    /// </summary>
    public interface ICompanyResource
    {
        long CompanyId { get; set; }
    }
}
