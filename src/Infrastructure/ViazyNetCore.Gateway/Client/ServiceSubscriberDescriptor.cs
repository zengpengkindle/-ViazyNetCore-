using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Gateway.Web;
using ViazyNetCore.Gateway.Web.Routing;

namespace ViazyNetCore.Gateway.Client
{
    public class ServiceSubscriberDescriptor
    {
        /// <summary>
        /// 服务地址描述符集合。
        /// </summary>
        public IEnumerable<ServiceAddressDescriptor> AddressDescriptors { get; set; }

        /// <summary>
        /// 服务描述符。
        /// </summary>
        public ServiceDescriptor ServiceDescriptor { get; set; }
    }
}
