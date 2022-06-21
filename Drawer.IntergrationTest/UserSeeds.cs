using Drawer.Contract.Authentication;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.IntergrationTest
{
    public class UserSeeds 
    {
        public class EmailPassword : IEnumerable<object[]>
        {
            private readonly IList<object[]> _objectList = new List<object[]>()
            {
                new object[]{ Master.Email, Master.Password },
                new object[]{ Admin.Email, Admin.Password },
                new object[]{ Manager.Email, Manager.Password },
            };

            public IEnumerator<object[]> GetEnumerator()
            {
                return _objectList.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public class EmailPasswordDisplayName : IEnumerable<object[]>
        {
            private readonly IList<object[]> _objectList = new List<object[]>()
            {
                new object[]{ Master.Email, Master.Password, Master.DisplayName },
                new object[]{ Admin.Email, Admin.Password, Admin.DisplayName },
                new object[]{ Manager.Email, Manager.Password, Manager.DisplayName },
            };

            public IEnumerator<object[]> GetEnumerator()
            {
                return _objectList.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }


        public IEnumerable<RegisterRequest> Users => _users;

        private readonly IList<RegisterRequest> _users = new List<RegisterRequest>
        {
            Master, Admin, Manager
        };

        public static RegisterRequest Master => new("master@master.com", "master12345", "master");

        public static RegisterRequest Admin => new("admin@admin.com", "admin12345", "admin");

        public static RegisterRequest Manager => new("manager@manager.com", "manager12345", "manager");


    }
}
