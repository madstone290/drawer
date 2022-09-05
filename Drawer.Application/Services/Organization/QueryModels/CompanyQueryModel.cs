using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Organization.QueryModels
{
    public class CompanyQueryModel
    {
        public long Id { get; set; }
        public long OwnerId { get; set; }
        public string Name { get; set; } = null!;
        public string? PhoneNumber { get; set; }
    }
}
