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
    public class AuditableEntity<TId> : Entity<TId>, IAuditable, ISoftDelete
    {
        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; } = default!;

        public DateTime? LastModifiedAt { get; set; }

        public string? LastModifiedBy { get; set; }

        public DateTime? DeletedAt { get; set; }

        public string? DeletedBy { get; set; }
    }
}
