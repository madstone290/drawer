using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Shared
{
    public static class DateTimeExtensions
    {
        public static DateTime KoreaNow => DateTime.UtcNow.ToKorea();

        public static DateTime KoreaToday => KoreaNow.Date;

        public static TimeSpan KoreaNowTime => KoreaNow.TimeOfDay;

        public static DateTime ToKorea(this DateTime utcTime)
        {
            return utcTime.AddHours(9);
        }

    }
}
