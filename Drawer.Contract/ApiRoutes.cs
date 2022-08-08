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

        /// <summary>
        /// 계정
        /// </summary>
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

        /// <summary>
        /// 사용자 본인
        /// </summary>
        public static class User
        {
            private const string Controller = "/User";
            private const string Index = Base + Controller;

            public const string Get = Index;
            public const string Update = Index;
            public const string UpdatePassword = Index + "/Password";

        }

        /// <summary>
        /// 사용자의 회사
        /// </summary>
        public static class Company
        {
            private const string Controller = "/Company";
            private const string Index = Base + Controller;

            public const string Create = Index;
            public const string Update = Index;
            public const string Get = Index;
            public const string GetMembers = Index + "/Members";
        }

        public static class Workplaces
        {
            public const string Create = "/Api/Workplaces";
            public const string Update = "/Api/Workplaces/{id}";
            public const string Delete = "/Api/Workplaces/{id}";
            public const string Get = "/Api/Workplaces/{id}";
            public const string GetList = "/Api/Workplaces";

            public const string BatchCreate = "/Api/Workplaces/Batch";
        }

        public static class Zones
        {
            public const string Create = "/Api/Zones";
            public const string Update = "/Api/Zones/{id}";
            public const string Delete = "/Api/Zones/{id}";
            public const string Get = "/Api/Zones/{id}";
            public const string GetList = "/Api/Zones";

            public const string BatchCreate = "/Api/Zones/Batch";
        }

        public static class Spots
        {
            public const string Create = "/Api/Spots";
            public const string Update = "/Api/Spots/{id}";
            public const string Delete = "/Api/Spots/{id}";
            public const string Get = "/Api/Spots/{id}";
            public const string GetList = "/Api/Spots";

            public const string BatchCreate = "/Api/Spots/Batch";
        }

        public static class Items
        {
            public const string Create =    "/Api/Items";
            public const string Update =    "/Api/Items/{id}";
            public const string Delete =    "/Api/Items/{id}";
            public const string Get =       "/Api/Items/{id}";
            public const string GetList =   "/Api/Items";

            public const string BatchCreate = "/Api/Items/Batch";
        }


        public static class Locations
        {
            public const string Create = "/Api/Locations";
            public const string Update = "/Api/Locations/{id}";
            public const string Delete = "/Api/Locations/{id}";
            public const string Get = "/Api/Locations/{id}";
            public const string GetList = "/Api/Locations";

            public const string BatchCreate = "/Api/Locations/Batch";
        }

        public static class InventoryItems
        {
            public const string Get = "/Api/InventoryItems";

            public const string BatchUpdate = "/Api/InventoryItems/Batch";
            public const string Update = "/Api/InventoryItems";
        }

        public static class Receipts
        {
            public const string Create = "/Api/Receipts";
            public const string Update = "/Api/Receipts/{id}";
            public const string Delete = "/Api/Receipts/{id}";
            public const string Get = "/Api/Receipts/{id}";
            public const string GetList = "/Api/Receipts";
        }

        public static class Issues
        {
            public const string Create = "/Api/Issues";
            public const string Update = "/Api/Issues/{id}";
            public const string Delete = "/Api/Issues/{id}";
            public const string Get = "/Api/Issues/{id}";
            public const string GetList = "/Api/Issues";
        }
    }
}
