using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Domain.Models
{
    public class Entity
    {
        public long Id { get; set; }

        public string Tenant { get; set; } = default!;

        public DateTime Created { get; set; }

        public string CreatedBy { get; set; } = default!;

        public DateTime? LastModified { get; set; }

        public string? LastModifiedBy { get; set; }
    }
}
