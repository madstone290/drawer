using Drawer.Contract.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.IntergrationTest.Seeds
{
    public static class CompanySeeds
    {
        public static CreateCompanyRequest MasterCompany => new("MasterCompany", "01-2345-6789");
    }
}
