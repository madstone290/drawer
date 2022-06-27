using Drawer.IntergrationTest.Seeds;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.IntergrationTest.TheoryData
{
    /// <summary>
    /// 사용자당 하나의 회사만 생성할 수 있으므로 각각 다른 사용자를 준비한다.
    /// </summary>
    public class CompanyControllerTestData
    {
        public class User1Company : IEnumerable<object[]>
        {
            private readonly IList<object[]> _objectList = new List<object[]>()
            {
                new object[]{ UserSeeds.User1.Email, UserSeeds.User1.Password , 
                    "google", "01-234-56789",
                    "apple", "01-234-56789" },
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
  

        public class User2Company : IEnumerable<object[]>
        {
            private readonly IList<object[]> _objectList = new List<object[]>()
            {
                new object[]{ UserSeeds.User2.Email, UserSeeds.User2.Password , "google", "01-234-56789" },
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

        public class User3Company : IEnumerable<object[]>
        {
            private readonly IList<object[]> _objectList = new List<object[]>()
            {
                new object[]{ UserSeeds.User3.Email, UserSeeds.User3.Password , "google", "01-234-56789" },
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

        public class User4Company : IEnumerable<object[]>
        {
            private readonly IList<object[]> _objectList = new List<object[]>()
            {
                new object[]{ UserSeeds.User4.Email, UserSeeds.User4.Password , "google", "01-234-56789" },
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

        public class User5Company : IEnumerable<object[]>
        {
            private readonly IList<object[]> _objectList = new List<object[]>()
            {
                new object[]{ UserSeeds.User5.Email, UserSeeds.User5.Password , "google", "01-234-56789" },
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
