using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Domain.Models
{
    /// <summary>
    /// 소프트 삭제 인터페이스
    /// </summary>
    public interface ISoftDelete
    {
        DateTime? DeletedAt { get; set; }

        string? DeletedBy { get; set; }
    }
}
