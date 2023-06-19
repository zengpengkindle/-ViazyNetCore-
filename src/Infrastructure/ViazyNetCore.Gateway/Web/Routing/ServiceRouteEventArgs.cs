using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Gateway.Web.Routing
{
    /// <summary>
    /// 服务路由事件参数。
    /// </summary>
    public class ServiceRouteEventArgs
    {
        public ServiceRouteEventArgs(ServiceRoute route)
        {
            Route = route;
        }

        /// <summary>
        /// 服务路由信息。
        /// </summary>
        public ServiceRoute Route { get; private set; }
    }

    /// <summary>
    /// 服务路由变更事件参数。
    /// </summary>
    public class ServiceRouteChangedEventArgs : ServiceRouteEventArgs
    {
        public ServiceRouteChangedEventArgs(ServiceRoute route, ServiceRoute oldRoute) : base(route)
        {
            OldRoute = oldRoute;
        }

        /// <summary>
        /// 旧的服务路由信息。
        /// </summary>
        public ServiceRoute OldRoute { get; set; }
    }
}
