using ClosedXML.Excel;
using ViazyNetCore.Formatter.Excel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Formatter.Excel.Attributes;
using System.Runtime.Intrinsics.X86;
using ClosedXML.Excel.Drawings;

namespace ViazyNetCore.Formatter.Excel.Extensions
{
    public static class ReportExtensions
    {
        public static async Task<DataTable> ToDataTableAsync<T>(this IEnumerable<T> data, string name)
        {
            var columns = GetColumnsFromModel(typeof(T)).ToDictionary(x => x.Name, x => x.Value);
            var table = new DataTable(name ?? typeof(T).Name);

            await Task.Run(() =>
            {
                foreach(var column in columns)
                {
                    table.Columns.Add(column.Key, Nullable.GetUnderlyingType(column.Value.PropertyDescriptor.PropertyType) ?? column.Value.PropertyDescriptor.PropertyType);
                }

                foreach(T item in data)
                {
                    var row = table.NewRow();
                    foreach(var prop in columns)
                    {
                        row[prop.Key] = prop.Value.IsImageCloumn ? DBNull.Value : PropertyExtensions.GetPropertyValue(item, prop.Value.Path) ?? DBNull.Value;
                    }

                    table.Rows.Add(row);
                }
            });

            return table;
        }

        internal static IEnumerable<Column> GetColumnsFromModel(Type parentClass, string parentName = null)
        {
            var properties = parentClass.GetProperties()
                       .Where(p => p.GetCustomAttributes<DisplayAttribute>().Any() || p.GetCustomAttributes<DisplayNameAttribute>().Any());


            var imageProperties = parentClass.GetProperties()
                       .Where(p => p.GetCustomAttributes<ImageColumnAttribute>().Any());

            foreach(var prop in properties)
            {
                var isImageCloumn = false;
                if(imageProperties.Contains(prop))
                {
                    isImageCloumn = true;
                }
                yield return new Column
                {
                    Name = prop.GetPropertyDisplayName(),
                    Value = new ColumnValue
                    {
                        IsImageCloumn = isImageCloumn,
                        Path = string.IsNullOrWhiteSpace(parentName) ? prop.Name : $"{parentName}.{prop.Name}",
                        PropertyDescriptor = prop.GetPropertyDescriptor()
                    }
                };
            }
        }

        public static async Task<byte[]> GenerateExcelForDataTableAsync<T>(this IEnumerable<T> data, string name, ExportOptions options = null, bool isDefaultTextDateType = false, bool adjustToContents = true)
        {
            options ??= ExportOptions.DefaultOptions;
            var tableHeaderColor = options.HasMarking ? 0xacb9ca : 0x3e90d1;
            var table = await data.ToDataTableAsync(name);

            var columns = table.Columns.Count;
            var rows = table.Rows.Count;

            using(var workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var ws = workbook.Worksheets.Add(table);

                ws.Tables.FirstOrDefault().Theme = XLTableTheme.None;

                ws.Author = "yuantuitui.com";
                ws.ShowGridLines = true;
                ws.Tables.FirstOrDefault().ShowAutoFilter = false;
                ws.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                if(!isDefaultTextDateType)
                {
                    ws.Range(1, 1, rows, columns).Style.NumberFormat.Format = "General";
                }
                ws.Style.Font.FontName = "微软雅黑";
                ws.Style.Font.FontSize = 10;
                ws.Rows().Height = 16;
                if(adjustToContents)
                {
                    ws.Columns().AdjustToContents();

                }
                /*表头样式*/
                var headerRange = ws.Range(1, 1, 1, columns);
                headerRange.Style.Fill.BackgroundColor = XLColor.FromArgb(tableHeaderColor);
                headerRange.Style.Font.FontSize = 10;
                headerRange.Style.Font.Bold = false;
                headerRange.Style.Font.FontColor = XLColor.FromArgb(0xffffff);
                ws.FirstRow().Height = 24;
                ws.SheetView.FreezeRows(1);

                if(!string.IsNullOrWhiteSpace(options.Marking))
                {
                    var markingRow = ws.Row(1).InsertRowsAbove(1);
                    markingRow.Height = 30;
                    markingRow.Style.Font.FontName = "微软雅黑";
                    markingRow.Style.Font.FontSize = 12;
                    markingRow.Style.Font.FontColor = XLColor.FromArgb(0x333333);
                    markingRow.Style.Font.Bold = false;
                    markingRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                    ws.Cell(1, 1).Value = options.Marking;
                    ws.Range(1, 1, 1, columns).Merge().Style.Fill.BackgroundColor = XLColor.FromArgb(0xffffff);
                }
                if(!string.IsNullOrWhiteSpace(options.Title))
                {
                    var titleRow = ws.Row(1).InsertRowsAbove(1);
                    titleRow.Height = 30;
                    titleRow.Style.Font.FontName = "微软雅黑";
                    titleRow.Style.Font.FontSize = 14;
                    titleRow.Style.Font.FontColor = XLColor.FromArgb(0xffffff);
                    titleRow.Style.Font.Bold = false;
                    ws.Cell(1, 1).Value = options.Title;
                    ws.Range(1, 1, 1, columns).Merge().Style.Fill.BackgroundColor = XLColor.FromArgb(0x3e90d1);
                }

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                return stream.ToArray();

            }
        }

        public static async Task<byte[]> GenerateExcelForDataTableInclubeImageAsync<T>(this IEnumerable<T> data, string name, ExportOptions options = null, bool isDefaultTextDateType = false, bool adjustToContents = true)
        {
            options ??= ExportOptions.DefaultOptions;
            var tableHeaderColor = options.HasMarking ? 0xacb9ca : 0x3e90d1;
            var table = await data.ToDataTableAsync(name);

            var columns = table.Columns.Count;
            var imageColumnModels = GetColumnsFromModel(typeof(T)).Where(p => p.Value.IsImageCloumn).ToList();

            var rows = table.Rows.Count;
            var hasImages = false;
            if(imageColumnModels?.Count > 0)
            {
                hasImages = true;
            }

            using(var workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var ws = workbook.Worksheets.Add(table);

                ws.Tables.FirstOrDefault().Theme = XLTableTheme.None;

                ws.Author = "yuantuitui.com";
                ws.ShowGridLines = true;
                ws.Tables.FirstOrDefault().ShowAutoFilter = false;
                ws.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                if(!isDefaultTextDateType)
                {
                    ws.Range(1, 1, rows, columns).Style.NumberFormat.Format = "#,##";
                }
                ws.Style.Font.FontName = "微软雅黑";
                ws.Style.Font.FontSize = 10;
                ws.Rows().Height = hasImages ? 80 : 16;
                if(adjustToContents)
                {
                    ws.Columns().AdjustToContents();

                }
                /*表头样式*/
                var headerRange = ws.Range(1, 1, 1, columns);
                headerRange.Style.Fill.BackgroundColor = XLColor.FromArgb(tableHeaderColor);
                headerRange.Style.Font.FontSize = 10;
                headerRange.Style.Font.Bold = false;
                headerRange.Style.Font.FontColor = XLColor.FromArgb(0xffffff);
                ws.FirstRow().Height = 24;
                ws.SheetView.FreezeRows(1);

                if(!string.IsNullOrWhiteSpace(options.Marking))
                {
                    var markingRow = ws.Row(1).InsertRowsAbove(1);
                    markingRow.Height = 30;
                    markingRow.Style.Font.FontName = "微软雅黑";
                    markingRow.Style.Font.FontSize = 12;
                    markingRow.Style.Font.FontColor = XLColor.FromArgb(0x333333);
                    markingRow.Style.Font.Bold = false;
                    markingRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                    ws.Cell(1, 1).Value = options.Marking;
                    ws.Range(1, 1, 1, columns).Merge().Style.Fill.BackgroundColor = XLColor.FromArgb(0xffffff);
                }
                if(!string.IsNullOrWhiteSpace(options.Title))
                {
                    var titleRow = ws.Row(1).InsertRowsAbove(1);
                    titleRow.Height = 30;
                    titleRow.Style.Font.FontName = "微软雅黑";
                    titleRow.Style.Font.FontSize = 14;
                    titleRow.Style.Font.FontColor = XLColor.FromArgb(0xffffff);
                    titleRow.Style.Font.Bold = false;
                    ws.Cell(1, 1).Value = options.Title;
                    ws.Range(1, 1, 1, columns).Merge().Style.Fill.BackgroundColor = XLColor.FromArgb(0x3e90d1);
                }

                if(imageColumnModels?.Count > 0)
                {
                    var index = 1;
                    foreach(var item in data)
                    {
                        index++;
                        foreach(var column in imageColumnModels)
                        {
                            var imagePath = PropertyExtensions.GetPropertyValue(item, column.Value.Path)?.ToString();
                            var ordinal = table.Columns[column.Name]?.Ordinal;
                            if(ordinal.HasValue)
                            {
                                try
                                {
                                    if(!string.IsNullOrWhiteSpace(imagePath))
                                    {
                                        var xlPic = ws.AddImgLink(imagePath);
                                        xlPic.MoveTo(ws.Cell(index, ordinal.Value+1)).WithSize(80,80);
                                    }
                                }
                                catch { }
                            }
                        }
                    }
                }

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                return stream.ToArray();

            }
        }

        public static async Task<byte[]> GenerateCsvForDataAsync<T>(this IEnumerable<T> data)
        {
            var builder = new StringBuilder();
            var stringWriter = new StringWriter(builder);

            await Task.Run(() =>
            {
                var columns = GetColumnsFromModel(typeof(T)).ToDictionary(x => x.Name, x => x.Value);

                foreach(var column in columns)
                {
                    stringWriter.Write(column.Key);
                    stringWriter.Write(", ");
                }
                stringWriter.WriteLine();

                foreach(T item in data)
                {
                    var properties = item.GetType().GetProperties();
                    foreach(var prop in columns)
                    {
                        stringWriter.Write(PropertyExtensions.GetPropertyValue(item, prop.Value.Path));
                        stringWriter.Write(", ");
                    }
                    stringWriter.WriteLine();
                }
            });

            return Encoding.UTF8.GetBytes(builder.ToString());
        }
    }
}
