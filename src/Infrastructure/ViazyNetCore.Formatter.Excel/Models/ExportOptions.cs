namespace ViazyNetCore.Formatter.Excel.Models
{
    public class ExportOptions
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 标注
        /// </summary>
        public string Marking { get; set; }
        /// <summary>
        /// Sheet
        /// </summary>
        public string Sheet { get; set; } = "Sheet1";

        public bool HasMarking => !string.IsNullOrWhiteSpace(Title);
        public static ExportOptions DefaultOptions => new ExportOptions()
        {
            Title = "",
            Marking = "",
        };
    }
}
