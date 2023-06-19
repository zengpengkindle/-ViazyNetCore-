using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Gateway.Web
{
    public interface IAddressSelector
    {
        /// <summary>
        /// 选择一个地址。
        /// </summary>
        /// <param name="context">地址选择上下文。</param>
        /// <returns>地址模型。</returns>
        ValueTask<AddressModel> SelectAsync(AddressSelectContext context);
    }

    /// <summary>
    /// 地址选择上下文。
    /// </summary>
    public class AddressSelectContext
    {
        /// <summary>
        /// 服务描述符。
        /// </summary>
        public ServiceDescriptor Descriptor { get; set; }

        /// <summary>
        /// 哈希参数
        /// </summary>
        public string Item { get; set; }
        /// <summary>
        /// 服务可用地址。
        /// </summary>
        public IEnumerable<AddressModel> Address { get; set; }
    }
}
