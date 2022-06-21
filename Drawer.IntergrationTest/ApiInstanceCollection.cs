using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Drawer.IntergrationTest
{
    [CollectionDefinition(Default)]
    public class ApiInstanceCollection : ICollectionFixture<ApiInstance>
    {
        public const string Default = "ApiInstance";
    }
}
