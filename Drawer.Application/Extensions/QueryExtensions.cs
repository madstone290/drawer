using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Helpers
{
    public static class QueryExtensions
    {
        /// <summary>
        /// 문자열에 쿼리 파라미터를 추가한다.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string AddQueryParam(this string uri, string name, string value)
        {
            bool hasQuery = uri.Contains("?");

            if(hasQuery)
            {
                return uri + "&" + name + "=" + value;
            }
            else
            {
                return uri + "?" + name + "=" + value;
            }
        }
    }
}
