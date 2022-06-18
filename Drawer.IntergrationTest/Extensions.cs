using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.IntergrationTest
{
    public static class Extensions
    {
        public static void SetBearerToken(this HttpRequestMessage request, string token)
        {
            request.Headers.Add("Authorization", $"bearer {token}");
        }

    }
}
