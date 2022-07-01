using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Contract.Organization
{
    public record UpdateCompanyRequest(string Name, string? PhoneNumber);
}
