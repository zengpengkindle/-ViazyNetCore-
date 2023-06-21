using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Gateway.Client
{
    /// <summary>
    /// 服务订阅者工厂接口
    /// </summary>
    public interface IServiceSubscriberFactory
    {
        /// <summary>
        /// 根据服务描述创建服务订阅者
        /// </summary>
        /// <param name="descriptors"></param>
        /// <returns></returns>
        Task<IEnumerable<ServiceSubscriber>> CreateServiceSubscribersAsync(IEnumerable<ServiceSubscriberDescriptor> descriptors);
    }
}
