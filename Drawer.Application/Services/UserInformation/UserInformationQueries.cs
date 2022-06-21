using Drawer.Application.Services.UserInformation.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.UserInformation
{
    public static class UserInformationQueries
    {
        public static GetUserQuery GetUser(string email) 
            => new(email);

    }
}
