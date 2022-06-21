using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Contract
{
    public static class ApiRoutes
    {
        private const string Base = "/Api";

        public static class User
        {
            private const string Controller = "/User";

            public const string GetUser = Base + Controller;
            public const string UpdateUser = Base + Controller;
            public const string UpdatePassword = Base + Controller + "/Password";

        }
    }
}
