
using MongoDB.Driver;

namespace System
{

    /// <summary>
    /// 定义一个仓储索引。
    /// </summary>
    public interface IRepositoryIndex
    {
        /// <summary>
        /// 获取索引的唯一编号。
        /// </summary>
        int Id { get; }

        /// <summary>
        /// 获取一个值，表示是否为倒序索引。
        /// </summary>
        bool Descending { get; }
    }

    /// <summary>
    /// 定义一个仓储索引。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class RepositoryIndexAttribute : Attribute, IRepositoryIndex
    {
        /// <inheritdoc />
        public int Id { get; }

        /// <summary>
        /// 获取或设置一个值，表示是否为倒序索引。
        /// </summary>
        public bool Descending { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示是否为唯一索引。
        /// </summary>
        public bool Unique { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示是否为稀疏索引。
        /// </summary>
        /// <remarks>当指定为 <see langword="true"/> 值时，表示对文档中不存在的字段数据不启用索引。</remarks>
        public bool Sparse { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示是否以后台方式创建索引。
        /// </summary>
        public bool Background { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示是否 TTL 索引。
        /// </summary>
        /// <remarks>
        /// <para>设定此值的索引必须在日期类型的索引上。</para>
        /// <para>到期后文档将被自动删除。</para>
        /// <para>不能保证过期的数据将在过期后立即删除，删除过期文档的后台任务每 60 秒运行一次。</para>
        /// </remarks>
        public int ExpireAfterSeconds { get; set; }

        /// <summary>
        /// 初始化一个 <see cref="RepositoryIndexAttribute"/> 类的新实例。
        /// </summary>
        /// <param name="id">唯一编号。</param>
        public RepositoryIndexAttribute(int id = -1)
        {
            this.Id = id;
        }

        ///// <summary>
        ///// 获取或设置一个值，表示索引版本。
        ///// </summary>
        //public int Version { get; set; }

        ///// <summary>
        ///// 获取或设置一个值，表示是否不区分大小写索引。
        ///// </summary>
        //public bool CaseInsensitive { get; set; }

        //public string BuildIndexName(IEnumerable<PropertyMapper> properties)
        //{
        //    //- 'IX_Key1_key2'
        //    var name = "IX_" + string.Join('_', properties.OrderBy(p => p.Name).Select(p => p.Name));

        //    return name;
        //    //if(this.Unique) name += "+U";
        //    //if(this.Sparse) name += "+S";
        //    //if(this.ExpireAfterSeconds > 0) name += "+T";


        //}

        /// <summary>
        /// 创建索引配置。
        /// </summary>
        /// <returns>索引配置。</returns>
        public CreateIndexOptions CreateIndexOptions()
        {
            return new()
            {
                Unique = this.Unique ? true : null,
                Sparse = this.Sparse ? true : null,
                ExpireAfter = this.ExpireAfterSeconds > 0 ? TimeSpan.FromSeconds(this.ExpireAfterSeconds) : null,
                Background = this.Background ? true : null,
            };
        }
    }

    /// <summary>
    /// 定义一个仓储复合索引。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class CompoundIndexAttribute : Attribute, IRepositoryIndex
    {
        /// <inheritdoc />
        public int Id { get; }

        /// <inheritdoc />
        public bool Descending { get; }

        /// <summary>
        /// 初始化一个 <see cref="RepositoryIndexAttribute"/> 类的新实例。
        /// </summary>
        /// <param name="id">唯一编号。</param>
        /// <param name="descending">是否为倒序索引。</param>
        public CompoundIndexAttribute(int id, bool descending = false)
        {
            this.Id = id;
            this.Descending = descending;
        }
    }
}
