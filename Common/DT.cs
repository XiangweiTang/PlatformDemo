using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class DT
    {
        public static string ToStringLog(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }
        public static string ToStringPathShort(this DateTime dt)
        {
            return dt.ToString("yyyyMMdd");
        }
        public static string ToStringPathMiddle(this DateTime dt)
        {
            return dt.ToString("yyyyMMdd_HHmmss");
        }
        public static string ToStringPathLong(this DateTime dt)
        {
            return dt.ToString("yyyyMMdd_HHmmssfff");
        }
    }
}
