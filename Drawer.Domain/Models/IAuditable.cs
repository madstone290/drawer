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
        DateTime CreatedAt { get; set; }

        string CreatedBy { get; set; }

        DateTime? LastModifiedAt { get; set; }

        string? LastModifiedBy { get; set; }
    }
}
