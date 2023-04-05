using System.ComponentModel.DataAnnotations;

namespace System
{
    /// <summary>
    /// 表示一个表格数据源。
    /// </summary>
    public abstract class PageData
    {
        /// <summary>
        /// 初始化一个 <see cref="PageData{TModel}"/> 类的新实例。
        /// </summary>
        protected PageData() { }

        /// <summary>
        /// 获取或设置行的总数。
        /// </summary>
        public long Total { get; set; }

        /// <summary>
        /// 创建一个表格数据源。
        /// </summary>
        /// <typeparam name="TModel">数据源的行数据类型。</typeparam>
        /// <param name="rows">所有数据的行。可以一个 <see langword="null"/> 值。</param>
        /// <returns>一个 <see cref="PageData{TModel}"/> 的新实例。</returns>
        public static PageData<TModel> Create<TModel>(List<TModel>? rows) => Create(rows, rows is null ? 0 : rows.Count);

        /// <summary>
        /// 创建一个表格数据源。
        /// </summary>
        /// <typeparam name="TModel">数据源的行数据类型。</typeparam>
        /// <param name="total">行的总数。</param>
        /// <param name="rows">所有数据的行。可以一个 <see langword="null"/> 值。</param>
        /// <returns>一个 <see cref="PageData{TModel}"/> 的新实例。</returns>
        public static PageData<TModel> Create<TModel>(List<TModel>? rows, long total) => new(rows, total);
    }

    /// <summary>
    /// 表示一个表格数据源。
    /// </summary>
    /// <typeparam name="TModel">数据源的行数据类型。</typeparam>
    public class PageData<TModel> : PageData/*, IEnumerable<TModel>*/
    {

        /// <summary>
        /// 初始化一个 <see cref="PageData{TModel}"/> 类的新实例。
        /// </summary>
        public PageData() : this(Array.Empty<TModel>(), 0) { }

        /// <summary>
        /// 提供行的总数和数据，初始化一个 <see cref="PageData{TModel}"/> 类的新实例。
        /// </summary>
        /// <param name="total">行的总数。</param>
        /// <param name="rows">所有数据的行。可以一个 <see langword="null"/> 值。</param>
        public PageData(TModel[]? rows, long total)
        {
            if (total < 0) throw new ArgumentOutOfRangeException(nameof(total));
            if (rows is null) rows = Array.Empty<TModel>();
            if (rows.Length > total) throw new ArgumentException("The length of rows exceed the total number of rows.", nameof(rows));

            this.Total = total;
            this.Rows = rows.ToList();
        }
        /// <summary>
        /// 提供行的总数和数据，初始化一个 <see cref="PageData{TModel}"/> 类的新实例。
        /// </summary>
        /// <param name="total">行的总数。</param>
        /// <param name="rows">所有数据的行。可以一个 <see langword="null"/> 值。</param>
        public PageData(List<TModel>? rows, long total)
        {
            if (total < 0) throw new ArgumentOutOfRangeException(nameof(total));
            if (rows is null) rows = new List<TModel>();
            if (rows?.Count > total) throw new ArgumentException("The length of rows exceed the total number of rows.", nameof(rows));

            this.Total = total;
            this.Rows = rows;
        }

        /// <summary>
        /// 获取指定索引的数据。
        /// </summary>
        /// <param name="index">数据的索引。</param>
        /// <returns>数据。</returns>
        public TModel this[int index] => this.Rows[index];

        /// <summary>
        /// 获取行的数据。
        /// </summary>
        [Required]
        public List<TModel> Rows { get; set; }
    }

    public class MorePageData<TModel>
    {
        /// <summary>
        /// 获取行的数据。
        /// </summary>
        [Required]
        public List<TModel> Rows { get; set; }

        public bool HasMore { get; set; }
    }
}
