using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.ObjectPool;

namespace System.Tests
{
    public class ConsoleTable
    {
        private readonly static StringBuilderPooledObjectPolicy Pooled = new StringBuilderPooledObjectPolicy();

        private readonly static HashSet<Type> NumericTypes = new HashSet<Type>
        {
            typeof(int),  typeof(double),  typeof(decimal),
            typeof(long), typeof(short),   typeof(sbyte),
            typeof(byte), typeof(ulong),   typeof(ushort),
            typeof(uint), typeof(float)
        };

        /// <summary>
        /// 获取列集合。
        /// </summary>
        public IList<object> Columns { get; }
        /// <summary>
        /// 获取行集合。
        /// </summary>
        public IList<object[]> Rows { get; }

        /// <summary>
        /// 获取或设置一个值，表示列的数据类型。
        /// </summary>
        public Type[] ColumnTypes { get; set; }

        /// <summary>
        /// 初始化一个 <see cref="ConsoleTable"/> 类的新实例。
        /// </summary>
        /// <param name="columns">列集合。</param>
        public ConsoleTable(params string[] columns)
        {
            this.Rows = new List<object[]>();
            this.Columns = new List<object>(columns);
        }

        /// <summary>
        /// 批量添加列。
        /// </summary>
        /// <param name="names">列名集合。</param>
        /// <returns>控制台表格。</returns>
        public ConsoleTable AddColumns(IEnumerable<string> names)
        {
            foreach (var name in names)
                this.Columns.Add(name);
            return this;
        }

        /// <summary>
        /// 添加一个新行。
        /// </summary>
        /// <param name="values">行的值。</param>
        /// <returns>控制台表格。</returns>
        public ConsoleTable NewRow(params object[] values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            if (!this.Columns.Any())
                throw new Exception("Please set the columns first");

            if (this.Columns.Count != values.Length)
                throw new Exception(
                    $"The number columns in the row ({this.Columns.Count}) does not match the values ({values.Length}");

            this.Rows.Add(values);
            return this;
        }

        /// <summary>
        /// 创建基于对象集合的控制台表格。
        /// </summary>
        /// <typeparam name="T">对象的数据类型。</typeparam>
        /// <param name="values">对象集合。</param>
        /// <returns>控制台表格。</returns>
        public static ConsoleTable From<T>(IEnumerable<T> values)
        {
            var typeMapper = TypeMapper<T>.Instance;
            var table = new ConsoleTable
            {
                ColumnTypes = typeMapper.Properties.Select(p => p.Property.PropertyType).ToArray()
            };

            table.AddColumns(typeMapper.Properties.Select(p => p.Property.Name));

            foreach (var propertyValues in values.Select(value => typeMapper.Properties.Select(p => p.Property.GetValue(value))))
                table.NewRow(propertyValues.ToArray());

            return table;
        }

        private string ToString<T>(List<int> columnLengths, List<string> columnAlignment, T[] array)
        {
            var builder = Pooled.Create();
            try
            {
                builder.Append(" | ");
                for (int i = 0; i < array.Length; i++)
                {
                    if (i > 0) builder.Append(" | ");
                    var value = array[i]?.ToString() ?? string.Empty;
                    var r = value.ToCharArray().Select(c => c.ToString().GetDataLength()).Where(s => s == 2).Count();

                    builder.AppendFormat("{0," + columnAlignment[i] + (columnLengths[i] - r) + "}", value);
                }
                builder.Append(" | ");
                return builder.ToString();
            }
            finally
            {
                Pooled.Return(builder);
            }
        }

        /// <summary>
        /// 输出文本格式的控制台表格。
        /// </summary>
        /// <param name="enableCount">是否启用行数统计。</param>
        /// <returns>字符串格式的控制台表格。</returns>
        public virtual string ToString(bool enableCount)
        {

            var builder = Pooled.Create();
            try
            {
                // find the longest column by searching each row
                var columnLengths = this.ColumnLengths();

                // set right alinment if is a number
                var columnAlignment = Enumerable.Range(0, this.Columns.Count)
                    .Select(this.GetNumberAlignment)
                    .ToList();

                // create the string format with padding
                //var format = Enumerable.Range(0, Columns.Count)
                //    .Select(i => " | {" + i + "," + columnAlignment[i] + columnLengths[i] + "}")
                //    .Aggregate((s, a) => s + a) + " |";

                var rows = this.Rows.Select(r => this.ToString(columnLengths, columnAlignment, r));
                // find the longest formatted line
                var maxRowLength = Math.Max(0, this.Rows.Any() ? rows.Max(row => row.GetDataLength()) : 0);
                var columnHeaders = this.ToString(columnLengths, columnAlignment, this.Columns.ToArray());

                // longest line is greater of formatted columnHeader and longest row
                var longestLine = Math.Max(maxRowLength, columnHeaders.GetDataLength());

                // add each row
                //var results = Rows.Select(row => string.Format(format, row)).ToList();

                // create the divider
                var divider = " " + string.Join("", Enumerable.Repeat("-", longestLine - 2)) + " ";

                builder.AppendLine(divider);
                builder.AppendLine(columnHeaders);

                foreach (var row in rows)
                {
                    builder.AppendLine(divider);
                    builder.AppendLine(row);
                }

                builder.AppendLine(divider);

                if (enableCount)
                {
                    builder.AppendLine("");
                    builder.AppendFormat(" Count: {0}", this.Rows.Count);
                }

                return builder.ToString();
            }
            finally
            {
                Pooled.Return(builder);
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.ToString(true);
        }

        private string GetNumberAlignment(int i)
        {
            return this.ColumnTypes != null && NumericTypes.Contains(this.ColumnTypes[i])
                ? ""
                : "-";
        }

        private List<int> ColumnLengths()
        {
            var columnLengths = this.Columns
                .Select((t, i) => this.Rows.Select(x => x[i])
                    .Union(new[] { this.Columns[i] })
                    .Where(x => x != null)
                    .Select(x => x.ToString()!.GetDataLength()).Max())
                .ToList();
            return columnLengths;
        }
    }
}
