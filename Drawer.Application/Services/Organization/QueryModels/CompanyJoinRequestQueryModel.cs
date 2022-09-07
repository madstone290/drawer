using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Organization.QueryModels
{
    public class CompanyJoinRequestQueryModel
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public long UserId { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime RequestTimeUtc { get; set; }
        public bool IsHandled { get; set; }
    }
}
