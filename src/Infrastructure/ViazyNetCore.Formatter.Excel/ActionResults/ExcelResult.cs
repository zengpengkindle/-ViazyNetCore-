using ViazyNetCore.Formatter.Excel.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using ViazyNetCore.Formatter.Excel.Models;
using ClosedXML.Excel;

namespace ViazyNetCore.Formatter.Excel
{
    public class ExcelResult<T> : IActionResult where T : class
    {
        private readonly IEnumerable<T> _data;

        public ExcelResult(IEnumerable<T> data, string fileName, string sheetName = "Sheet1", ExportOptions options = null,bool AdjustToContents=true)
        {
            _data = data;
            SheetName = sheetName;
            FileName = fileName;
            Options = options;
            adjustToContents = AdjustToContents;
        }

        public string SheetName { get; }
        public string FileName { get; }
        public bool adjustToContents { get; }
        public ExportOptions Options { get; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            try
            {
                var excelBytes = await _data.GenerateExcelForDataTableInclubeImageAsync(SheetName, Options, false, adjustToContents);
                WriteExcelFileAsync(context.HttpContext, excelBytes);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                var errorBytes = await new List<T>().GenerateExcelForDataTableInclubeImageAsync(SheetName, Options);
                WriteExcelFileAsync(context.HttpContext, errorBytes);
            }
        }

        private async void WriteExcelFileAsync(HttpContext context, byte[] bytes)
        {
            context.Response.Headers["Access-Control-Expose-Headers"] = "content-disposition";
            context.Response.Headers["content-disposition"] = $"attachment; filename={HttpUtility.UrlEncode(FileName)}.xlsx";

            await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
        }
    }

    public class ExcelResult : IActionResult
    {
        private readonly string _fileName;
        private readonly byte[] _buffer;

        public ExcelResult(string fileName, byte[] buffer)
        {
            _fileName = fileName;
            _buffer = buffer;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            try
            {
                await WriteExcelFileAsync(context.HttpContext, _buffer);
            }
            catch (Exception e)
            {
                await WriteExcelFileAsync(context.HttpContext, null);
            }
        }

        private async Task WriteExcelFileAsync(HttpContext context, byte[] bytes)
        {
            context.Response.Headers["Access-Control-Expose-Headers"] = "content-disposition";
            context.Response.Headers["content-disposition"] = $"attachment; filename={HttpUtility.UrlEncode(_fileName)}.xlsx";

            await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
        }
    }
}
