using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Gateway.Client
{
    public interface IServiceSubscribeManager
    {

        /// <summary>
        /// 获取所有可用的服务订阅者信息。
        /// </summary>
        /// <returns>服务路由集合。</returns>
        Task<IEnumerable<ServiceSubscriber>> GetSubscribersAsync();

        /// <summary>
        /// 设置服务订阅者。
        /// </summary>
        /// <param name="routes">服务路由集合。</param>
        /// <returns>一个任务。</returns>
        Task SetSubscribersAsync(IEnumerable<ServiceSubscriber> subscibers);


        /// <summary>
        /// 清空所有的服务订阅者。
        /// </summary>
        /// <returns>一个任务。</returns>
        Task ClearAsync();
    }
}
