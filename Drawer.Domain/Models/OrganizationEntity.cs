using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Domain.Models
{
    /// <summary>
    /// 조직에 포함되는 엔티티
    /// </summary>
    public class OrganizationEntity : Entity
    {
        public string Tenant { get; set; } = default!;

        public DateTime Created { get; set; }

        public string CreatedBy { get; set; } = default!;

        public DateTime? LastModified { get; set; }

        public string? LastModifiedBy { get; set; }
    }
}
