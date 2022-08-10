using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Organization.QueryModels
{
    public class CompanyMemberQueryModel
    {
        public string CompanyId { get; set; } = null!;
        public string UserId { get; set; } = null!;
    }
}
