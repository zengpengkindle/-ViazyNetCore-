using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class WebExtensions
    {
        public static DateTime ToEndTime(this DateTime dateTime)
        {
            if(dateTime.Hour == 0 && dateTime.Minute == 0 && dateTime.Second == 0 && dateTime.Millisecond == 0)
                return dateTime.AddDays(1);
            return dateTime;
        }
    }
}
