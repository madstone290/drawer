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

        public const string WorkplaceHome = "/Workplace";
        public const string WorkplaceBatchEdit = "/Workplace/BatchEdit";
        public const string WorkplaceAdd = "/Workplace/Add";
        public const string WorkplaceUpdate = "/Workplace/Update/{id}";
        public const string WorkplaceView = "/Workplace/View/{id}";
        public const string WorkplaceEdit = "/Workplace/Edit/{id}";

        public const string ZoneHome = "/Zone/Home";
        public const string ZoneBatchEdit = "/Zone/BatchEdit";
        public const string ZoneAdd = "/Zone/Add";
        public const string ZoneUpdate = "/Zone/Update/{id}";
        public const string ZoneView = "/Zone/View/{id}";
        public const string ZoneEdit = "/Zone/Edit/{id}";

        public const string SpotHome = "/Spot/Home";
        public const string SpotBatchEdit = "/Spot/BatchEdit";
        public const string SpotAdd = "/Spot/Add";
        public const string SpotUpdate = "/Spot/Update/{id}";
        public const string SpotView = "/Spot/View/{id}";
        public const string SpotEdit = "/Spot/Edit/{id}";

        public const string ItemHome = "/Item";
        public const string ItemBatchEdit = "/Item/BatchEdit";
        public const string ItemAdd = "/Item/Add";
        public const string ItemUpdate = "/Item/Update/{id}";
        public const string ItemView = "/Item/View/{id}";
        public const string ItemEdit = "/Item/Edit/{id}";
        
    }
}

