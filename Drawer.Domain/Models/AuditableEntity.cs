using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Domain.Models
{
    /// <summary>
    /// 감사기능이 포함된 엔티티
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public class AuditableEntity<TId> : Entity<TId>, IAuditable
    {
        public Guid AuditId { get; set; } = Guid.NewGuid();
    }
}
