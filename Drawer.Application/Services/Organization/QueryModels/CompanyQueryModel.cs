using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Organization.QueryModels
{
    public class CompanyQueryModel
    {
        public string Id { get; set; } = null!;
        public string OwnerId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? PhoneNumber { get; set; }
    }
}
