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

        public const string ItemHome = "/Item";
        public const string ItemBatchEdit = "/Item/BatchEdit";
        public const string ItemAdd = "/Item/Add";
        public const string ItemUpdate = "/Item/Update/{id}";
        public const string ItemView = "/Item/View/{id}";
        public const string ItemEdit = "/Item/Edit/{id}";

        public const string LocationHome = "/Location";
        public const string LocationBatchEdit = "/Location/BatchEdit";
        public const string LocationAdd = "/Location/Add";
        public const string LocationUpdate = "/Location/Update/{id}";
        public const string LocationView = "/Location/View/{id}";
        public const string LocationEdit = "/Location/Edit/{id}";

        public const string InventoryStatusHome = "/ItemInventoryHome";
        public const string ItemLocationInventoryHome = "/ItemLocationInventoryHome";
        public const string LocationItemInventoryHome = "/LocationItemInventoryHome";

        public const string ReceiptHome = "/Receipt";
        public const string ReceiptBatchAdd = "/Receipt/BatchAdd";
        public const string ReceiptAdd = "/Receipt/Add";
        public const string ReceiptUpdate = "/Receipt/Update/{id}";
        public const string ReceiptView = "/Receipt/View/{id}";
        public const string ReceiptEdit = "/Receipt/Edit/{id}";


        public const string IssueHome = "/Issue";
        public const string IssueBatchAdd = "/Issue/BatchAdd";
        public const string IssueAdd = "/Issue/Add";
        public const string IssueUpdate = "/Issue/Update/{id}";
        public const string IssueView = "/Issue/View/{id}";
        public const string IssueEdit = "/Issue/Edit/{id}";

        public const string TransferHome = "/Transfer";


        public const string LayoutHome = "/Layout";
        public const string LayoutEdit = "/Layout/Edit";
    }
}

