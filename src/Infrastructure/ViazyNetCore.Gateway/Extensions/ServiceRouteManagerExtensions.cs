using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Gateway.Web;

namespace ViazyNetCore.Gateway.Client
{
    public static class ServiceRouteManagerExtensions
    {
        /// <summary>
        /// 根据服务Id获取一个服务订阅者。
        /// </summary>
        /// <param name="serviceRouteManager">服务订阅管理者。</param>
        /// <param name="serviceId">服务Id。</param>
        /// <returns>服务路由。</returns>
        public static async Task<ServiceSubscriber> GetAsync(this IServiceSubscribeManager serviceSubscribeManager, string serviceId)
        {
            return (await serviceSubscribeManager.GetSubscribersAsync()).SingleOrDefault(i => i.ServiceDescriptor.Id == serviceId);
        }

        /// <summary>
        /// 获取地址
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<AddressModel>> GetAddressAsync(this IServiceSubscribeManager serviceSubscribeManager, string condition = null)
        {
            var subscribers = await serviceSubscribeManager.GetSubscribersAsync();
            if (condition != null)
            {
                if (!condition.IsIP())
                {
                    subscribers = subscribers.Where(p => p.ServiceDescriptor.Id == condition);
                }
                else
                {
                    subscribers = subscribers.Where(p => p.Address.Any(m => m.ToString() == condition));
                }
            }
            Dictionary<string, AddressModel> result = new Dictionary<string, AddressModel>();
            foreach (var route in subscribers)
            {
                var addresses = route.Address;
                foreach (var address in addresses)
                {
                    if (!result.ContainsKey(address.ToString()))
                    {
                        result.Add(address.ToString(), address);
                    }
                }
            }
            return result.Values;
        }

        public static async Task<IEnumerable<ServiceDescriptor>> GetServiceDescriptorAsync(this IServiceSubscribeManager serviceSubscribeManager, string address, string serviceId = null)
        {
            var subscribers = await serviceSubscribeManager.GetSubscribersAsync();
            if (serviceId == null)
            {
                return subscribers.Where(p => p.Address.Any(m => m.ToString() == address))
                 .Select(p => p.ServiceDescriptor);
            }
            else
            {
                return subscribers.Where(p => p.ServiceDescriptor.Id == serviceId)
               .Select(p => p.ServiceDescriptor);
            }
        }
    }
}
