using Drawer.Application.Services.Organization.CommandModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.IntergrationTest.Seeds
{
    public static class CompanySeeds
    {
        public static CompanyAddUpdateCommandModel MasterCompany => new CompanyAddUpdateCommandModel()
        {
            Name = "MasterCompany",
            PhoneNumber = "01-2345-6789"
        };
    }
}
