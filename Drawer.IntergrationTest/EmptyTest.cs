using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Drawer.IntergrationTest
{
    [Collection(ApiInstanceCollection.Default)]
    public class EmptyTest
    {
        /// <summary>
        /// DB초기화 용으로 사용한다.
        /// </summary>
        [Fact]
        public void DoNothing()
        {

        }
    }
}
