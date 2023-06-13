using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Zookeeper.Web
{
    /// <summary>
    /// 定义一个元数据。
    /// </summary>
    public interface IMetadata
    {
        /// <summary>
        /// 获取元数据的名称。
        /// </summary>
        string Name { get; }
    }

    /// <summary>
    /// 表示一个元数据的基类。
    /// </summary>
    public abstract class MetadataBase : IMetadata
    {
        /// <inheritdoc />
        public abstract string Name { get; }

        /// <summary>
        /// 初始化一个 <see cref="MetadataBase"/> 类的新实例。
        /// </summary>
        protected MetadataBase() { }
    }
}
