using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Contract.Organization
{
    public record CreateCompanyResponse(string Id, string OwnerId, string Name, string? PhoneNumber);
}
