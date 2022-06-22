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

        public static class Account
        {
            private const string Controller = "/Account";
            public const string Index = Base + Controller;

            public const string Register = Index + "/Register";
            public const string ConfirmEmail = Index + "/ConfirmEmail";
            public const string VerifyEmail = Index + "/VerifyEmail";
            public const string Login = Index + "/Login";
            public const string Refresh = Index + "/Refresh";
            public const string SecurityTest = Index + "/SecurityTest";
        }

        public static class User
        {
            private const string Controller = "/User";

            public const string Index = Base + Controller;
            public const string GetUser = Index;
            public const string UpdateUser = Index;
            public const string UpdatePassword = Index + "/Password";

        }
    }
}
