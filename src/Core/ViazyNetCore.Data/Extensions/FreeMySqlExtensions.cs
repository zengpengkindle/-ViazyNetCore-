using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FreeSql
{
    public static class FreeMySqlExtensions
    {
        public static FreeSqlCloud<string> AsClound(this IFreeSql freeSql)
        {
            if (freeSql is FreeSqlCloud<string> freeSqlChoud)
            {
                return freeSqlChoud;
            }
            else
            {
                throw new ArgumentException($"当前FreeSql对象不是{nameof(FreeSqlCloud<string>)}");
            }
        }

        public static IFreeSql UseDb(this IFreeSql freeSql, string dbKey)
        {
            if (freeSql is FreeSqlCloud<string> freeSqlChoud)
            {
                return freeSqlChoud.Use(dbKey);
            }
            else
            {
                throw new ArgumentException($"当前FreeSql对象不是{nameof(FreeSqlCloud<string>)}");
            }
        }
    }
}
