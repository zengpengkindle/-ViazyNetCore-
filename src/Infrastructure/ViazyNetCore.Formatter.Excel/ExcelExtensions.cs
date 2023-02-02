using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ViazyNetCore.Formatter.Excel.Models;
using ClosedXML.Excel;

namespace ViazyNetCore.Formatter.Excel
{
    public static class ExcelExtensions
    {
        class Column
        {
            public string Name { get; set; }
            public ColumnValue Value { get; set; }
        }

        class ColumnValue
        {
            public string Path { get; set; }
            public PropertyDescriptor PropertyDescriptor { get; set; }
        }

        private static async Task<DataTable> ToDataTableAsync<T>(this IEnumerable<T> data, string name)
        {
            var columns = GetColumnsFromModel(typeof(T)).ToDictionary(x => x.Name, x => x.Value);
            var table = new DataTable(name ?? typeof(T).Name);

            await Task.Run(() =>
            {
                foreach (var column in columns)
                {
                    table.Columns.Add(column.Key, Nullable.GetUnderlyingType(column.Value.PropertyDescriptor.PropertyType) ?? column.Value.PropertyDescriptor.PropertyType);
                }

                foreach (T item in data)
                {
                    var row = table.NewRow();
                    foreach (var prop in columns)
                    {
                        row[prop.Key] = GetPropertyValue(item, prop.Value.Path) ?? DBNull.Value;
                    }

                    table.Rows.Add(row);
                }
            });

            return table;
        }

        private static IEnumerable<Column> GetColumnsFromModel(Type parentClass, string parentName = null)
        {
            var properties = parentClass.GetProperties()
                       .Where(p => p.GetCustomAttributes<DisplayAttribute>().Any() ||
                                   p.GetCustomAttributes<DisplayNameAttribute>().Any());

            foreach (var prop in properties)
            {

                yield return new Column
                {
                    Name = prop.GetPropertyDisplayName(),
                    Value = new ColumnValue
                    {
                        Path = string.IsNullOrWhiteSpace(parentName) ? prop.Name : $"{parentName}.{prop.Name}",
                        PropertyDescriptor = prop.GetPropertyDescriptor()
                    }
                };
            }
        }

        /// <summary>
        /// 保存为excel文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="path">保存路径,必须是.xlsx文件</param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public static async Task ToExcel<T>(this IEnumerable<T> data, string path, string sheetName = "sheet1")
        {
            var table = await data.ToDataTableAsync(sheetName);

            using (var wb = new XLWorkbook(XLEventTracking.Disabled))
            {
                wb.Worksheets.Add(table).ColumnsUsed().AdjustToContents();
                wb.SaveAs(path);
            }
        }

        private static PropertyDescriptor GetPropertyDescriptor(this PropertyInfo propertyInfo)
        {
            return TypeDescriptor.GetProperties(propertyInfo.DeclaringType).Find(propertyInfo.Name, false);
        }

        private static string GetPropertyDisplayName(this PropertyInfo propertyInfo)
        {
            var propertyDescriptor = propertyInfo.GetPropertyDescriptor();
            var displayName = propertyInfo.IsDefined(typeof(DisplayAttribute), false)
                || propertyInfo.IsDefined(typeof(DisplayNameAttribute), false) ?
                (propertyInfo.GetCustomAttributes(typeof(DisplayAttribute),
                    false).Cast<DisplayAttribute>().SingleOrDefault()?.Name ?? propertyInfo.GetCustomAttributes(typeof(DisplayAttribute),
                    false).Cast<DisplayNameAttribute>().SingleOrDefault()?.DisplayName) : null;

            return displayName ?? propertyDescriptor.DisplayName ?? propertyDescriptor.Name;
        }

        private static object GetPropertyValue(object src, string propName)
        {
            if (src == null) throw new ArgumentException("Value cannot be null.", "src");
            if (propName == null) throw new ArgumentException("Value cannot be null.", "propName");

            if (propName.Contains("."))
            {
                var temp = propName.Split(new char[] { '.' }, 2);
                return GetPropertyValue(GetPropertyValue(src, temp[0]), temp[1]);
            }
            else
            {
                var prop = src.GetType().GetProperty(propName);
                return prop != null ? prop.GetValue(src, null) : null;
            }
        }


        /// <summary>
        /// 读取Excel数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="sheetNumber"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IEnumerable<T> ReadExcel<T>(this string path, int sheetNumber = 1, ReadOptions options = null)
            where T : class, new()
        {
            using var workBook = new XLWorkbook(path);
            return workBook.ReadTable<T>(sheetNumber, options);
        }
        /// <summary>
        /// 读取Excel数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <param name="sheetNumber"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IEnumerable<T> ReadExcel<T>(this Stream stream, int sheetNumber = 1, ReadOptions options = null)
            where T : class, new()
        {
            using var workBook = new XLWorkbook(stream);
            return workBook.ReadTable<T>(sheetNumber, options);
        }
    }
}
