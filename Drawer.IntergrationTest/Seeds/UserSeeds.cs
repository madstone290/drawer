using Drawer.Contract.Authentication;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.IntergrationTest.Seeds
{
    public class UserSeeds 
    {
        public IEnumerable<RegisterRequest> Users => _users;

        private readonly IList<RegisterRequest> _users = new List<RegisterRequest>
        {
            Master, 
            Admin, 
            SecurityUser, 
            RefreshUser1,
            RefreshUser2,
            LoginUser1,
            GetUser1,
            UpdateUser1, 
            PasswordUser1, 
            PasswordUser2
        };

        public static RegisterRequest Master => new("master@master.com", "master12345", "master");
        public static RegisterRequest Admin => new("admin@admin.com", "admin12345", "admin");
        public static RegisterRequest SecurityUser => new("SecurityUser@manager.com", "manager12345", "manager");
        public static RegisterRequest RefreshUser1 => new("RefreshUser1@manager.com", "manager12345", "manager");
        public static RegisterRequest RefreshUser2 => new("RefreshUser2@manager.com", "manager12345", "manager");
        public static RegisterRequest LoginUser1 => new("login@manager.com", "manager12345", "manager");
        public static RegisterRequest GetUser1 => new("getUser@master.com", "worker12345", "worker");
        public static RegisterRequest UpdateUser1 => new("worker1@master.com", "worker12345", "worker");
        public static RegisterRequest PasswordUser1 => new("worker2@admin.com", "worker12345", "worker");
        public static RegisterRequest PasswordUser2 => new("worker3@manager.com", "worker12345", "worker");

        public class LoginUser : IEnumerable<object[]>
        {
            private readonly IList<object[]> _objectList = new List<object[]>()
            {
                new object[]{ LoginUser1.Email, LoginUser1.Password },
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

        public class RefreshUser_1 : IEnumerable<object[]>
        {
            private readonly IList<object[]> _objectList = new List<object[]>()
            {
                new object[]{ RefreshUser1.Email, RefreshUser1.Password },
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

        public class RefreshUser_2 : IEnumerable<object[]>
        {
            private readonly IList<object[]> _objectList = new List<object[]>()
            {
                new object[]{ RefreshUser2.Email, RefreshUser2.Password },
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

        public class SecurityTestUser : IEnumerable<object[]>
        {
            private readonly IList<object[]> _objectList = new List<object[]>()
            {
                new object[]{ SecurityUser.Email, SecurityUser.Password },
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

        /// <summary>
        /// 사용자 조회 테스트
        /// </summary>
        public class GetUser : IEnumerable<object[]>
        {
            private readonly IList<object[]> _objectList = new List<object[]>()
            {
                new object[]{ GetUser1.Email, GetUser1.Password, GetUser1.DisplayName },
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

        /// <summary>
        /// 사용자정보 변경테스트에 사용
        /// </summary>
        public class UpdateUser : IEnumerable<object[]>
        {
            private readonly IList<object[]> _objectList = new List<object[]>()
            {
                new object[]{ UpdateUser1.Email, UpdateUser1.Password },
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

        /// <summary>
        /// 사용자 비밀번호 변경테스트에 사용
        /// </summary>
        public class UpdaterPassword_1 : IEnumerable<object[]>
        {
            private readonly IList<object[]> _objectList = new List<object[]>()
            {
                new object[]{ PasswordUser1.Email, PasswordUser1.Password },
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

        /// <summary>
        /// 사용자 비밀번호 변경테스트에 사용
        /// </summary>
        public class UpdaterPassword_2 : IEnumerable<object[]>
        {
            private readonly IList<object[]> _objectList = new List<object[]>()
            {
                new object[]{ PasswordUser2.Email, PasswordUser2.Password },
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



    }
}
