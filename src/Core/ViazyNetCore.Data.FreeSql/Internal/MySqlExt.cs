﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql.DataAnnotations;
using static FreeSql.SqlExtExtensions;

namespace FreeSql
{
    /// <summary>
    /// 自定义解析
    /// </summary>
    [ExpressionCall]
    public static class MySqlExt
    {
        static ThreadLocal<ExpressionCallContext> expContext = new();
        internal static ThreadLocal<List<ExpSbInfo>> expSb = new();
        internal static ExpSbInfo expSbLast => expSb.Value.Last();
        internal class ExpSbInfo
        {
            public StringBuilder Sb { get; } = new StringBuilder();
            public bool IsOver = false;
            public bool IsOrderBy = false;
            public bool IsDistinct = false;
        }
        internal static ISqlOver<TValue> Over<TValue>(string sqlFunc)
        {
            if (expSb.Value == null) expSb.Value = new List<ExpSbInfo>();
            expSb.Value.Add(new ExpSbInfo());
            expSbLast.Sb.Append(sqlFunc);
            return null;
        }

        /// <summary>
        /// match ( column )  against({0} IN BOOLEAN MODE)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="column"></param>
        /// <param name="keywords"></param>
        /// <returns></returns>
#pragma warning disable IDE0060 // 删除未使用的参数
        public static ISqlOver<decimal> MatchAgainst<T>(T column, string keywords)
#pragma warning restore IDE0060 // 删除未使用的参数
        {
            if (keywords.IsNull())
                return Over<decimal>("0");
            return Over<decimal>("(match (" + expContext.Value!.ParsedContent["column"] + ")  against({0} IN BOOLEAN MODE))".FormatMySql(keywords));
        }

        public static ISqlOver<int> Ceil<T>(T column)
        {
            return Over<int>("(CEIL (" + expContext.Value!.ParsedContent["column"] + "))");
        }

#pragma warning disable IDE0060 // 删除未使用的参数
        public static ISqlOver<int> Floor<T>(T column)
#pragma warning restore IDE0060 // 删除未使用的参数
        {
            return Over<int>("(Floor (" + expContext.Value!.ParsedContent["column"] + "))");
        }

        /// <summary>
        /// ifnull(feild1,defaultValue)
        /// <para>注意，defaultValue 如果是enum 有类型转换问题。</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="column"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
#pragma warning disable IDE0060 // 删除未使用的参数
        public static ISqlOver<T> IfNull<T>(T column, T defaultValue)
#pragma warning restore IDE0060 // 删除未使用的参数
        {
            return Over<T>("(ifnull (" + expContext.Value!.ParsedContent["column"] + ","+ expContext.Value.ParsedContent["defaultValue"] + "))");
        }

#pragma warning disable IDE0060 // 删除未使用的参数
        public static TValue? Value<TValue>(this ISqlOver<TValue> that)
#pragma warning restore IDE0060 // 删除未使用的参数
        {
            string text = expSbLast.Sb.ToString().TrimEnd(',');
            if (expSbLast.IsOver)
            {
                text += ")";
            }

            expSbLast.Sb.Clear();
            expSb.Value!.RemoveAt(expSb.Value.Count - 1);
            expContext.Value!.Result = text;
            return default;
        }

        public static ISqlOver<long> Count()
        {
            return Over<long>("count(1)");
        }
    }
}
