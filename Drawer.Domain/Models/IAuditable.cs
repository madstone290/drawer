using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Domain.Models
{
    /// <summary>
    /// 감사 인터페이스
    /// </summary>
    public interface IAuditable
    {
        Guid AuditId { get; set; }
    }
}
