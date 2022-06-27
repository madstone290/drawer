using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Contract.Organization
{
    public record CreateCompanyRequest(string Name, string? PhoneNumber);
}
