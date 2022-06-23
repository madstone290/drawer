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
    public class AuditableEntity<TId> : Entity<TId>
    {
        public DateTime Created { get; protected set; }

        public string CreatedBy { get; protected set; } = default!;

        public DateTime? LastModified { get; protected set; }

        public string? LastModifiedBy { get; protected set; }
    }
}
