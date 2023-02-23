using System.ComponentModel;

namespace ViazyNetCore.Formatter.Excel.Models
{
    internal class ColumnValue
    {
        public string? Path { get; set; }
        public PropertyDescriptor? PropertyDescriptor { get; set; }
        public bool IsImageCloumn { get; internal set; }
    }
}
