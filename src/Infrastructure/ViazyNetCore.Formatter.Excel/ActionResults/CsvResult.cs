using ViazyNetCore.Formatter.Excel.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ViazyNetCore.Formatter.Excel
{
    public class CsvResult<T> : IActionResult where T : class
    {
        private readonly IEnumerable<T> _data;

        public CsvResult(IEnumerable<T> data, string fileName)
        {
            _data = data;
            FileName = fileName;
        }

        public string FileName { get; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            try
            {
                var csvBytes = await _data.GenerateCsvForDataAsync();
                WriteExcelFileAsync(context.HttpContext, csvBytes);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                var errorBytes = await new List<T>().GenerateCsvForDataAsync();
                WriteExcelFileAsync(context.HttpContext, errorBytes);
            }
        }

        private async void WriteExcelFileAsync(HttpContext context, byte[] bytes)
        {
            context.Response.Headers["content-disposition"] = $"attachment; filename={FileName}.csv";
            await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
        }
    }
}
