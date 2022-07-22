namespace Drawer.Web.Pages
{
    public static class Paths
    {
        public const string Base = "/";

        public static class Account
        {
            public const string ConfirmEmail = "/Account/ConfirmEmail";
            public const string Login = "/Account/Login";
            public const string LoginHandler = "/Account/LoginHandler";
            public const string LoginCallback = "/Account/LoginCallback";
            public const string Logout = "/Account/Logout";
            public const string LogoutHandler = "/Account/LogoutHandler";
            public const string LogoutCallback = "/Account/LogoutCallback";
            public const string Register = "/Account/Register";
            public const string RegisterHandler = "/Account/RegisterHandler";
            public const string RegisterCompleted= "/Account/RegisterCompleted";
            public const string Refresh = "/Account/Refresh";
        }

        public static class User
        {
            public const string Settings = "/User/Settings";
        }

        public static class Statistics
        {
            public const string Dashboard = "/Statistics/Dashboard";
        }

        public static class Organization
        {
            public const string Company = "/Organization/Company";
        }

        public static class Locations
        {
            public const string Location = "/Location";
            public const string WorkPlaces = "/Locations/WorkPlaces";
            public const string Zones = "/Locations/Zones";
            public const string Spots = "/Locations/Spots";
        }

        public static class Items
        {
            public const string Item = "/Item";
        }
        
    }
}

