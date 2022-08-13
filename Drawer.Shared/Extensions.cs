using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Shared
{
    public static class DateTimeExtensions
    {
        public static DateTime KoreaToday => DateTime.UtcNow.ToKorea().Date;

        public static DateTime KoreaNow => DateTime.UtcNow.ToKorea();

        public static TimeSpan KoreaNowTime => DateTime.UtcNow.ToKorea().TimeOfDay;

        public static DateTime ToKorea(this DateTime utcTime)
        {
            return utcTime.AddHours(9);
        }

    }
}
