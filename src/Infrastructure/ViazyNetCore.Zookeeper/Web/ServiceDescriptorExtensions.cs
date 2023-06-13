using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Zookeeper.Web
{
    /// <summary>
    /// 服务描述符扩展方法。
    /// </summary>
    public static class ServiceDescriptorExtensions
    {
        /// <summary>
        /// 获取组名称。
        /// </summary>
        /// <param name="descriptor">服务描述符。</param>
        /// <returns>组名称。</returns>
        public static string GroupName(this ServiceDescriptor descriptor)
        {
            return descriptor.GetMetadata<string>("GroupName");
        }

        /// <summary>
        /// 设置组名称。
        /// </summary>
        /// <param name="descriptor">服务描述符。</param>
        /// <param name="groupName">组名称。</param>
        /// <returns>服务描述符。</returns>
        public static ServiceDescriptor GroupName(this ServiceDescriptor descriptor, string groupName)
        {
            descriptor.Metadatas["GroupName"] = groupName;
            return descriptor;
        }

        /// <summary>
        /// 设置是否等待执行。
        /// </summary>
        /// <param name="descriptor">服务描述符。</param>
        /// <param name="waitExecution">如果需要等待执行则为true，否则为false，默认为true。</param>
        /// <returns>服务描述符。</returns>
        public static ServiceDescriptor WaitExecution(this ServiceDescriptor descriptor, bool waitExecution)
        {
            descriptor.Metadatas["WaitExecution"] = waitExecution;
            return descriptor;
        }

        /// <summary>
        /// 获取负责人
        /// </summary>
        /// <param name="descriptor">服务描述符。</param>
        /// <param name="waitExecution">负责人名字</param>
        /// <returns>服务描述符。</returns>
        public static ServiceDescriptor Director(this ServiceDescriptor descriptor, string director)
        {
            descriptor.Metadatas["Director"] = director;
            return descriptor;
        }

        /// <summary>
        /// 设置是否启用授权
        /// </summary>
        /// <param name="descriptor">服务描述符。</param>
        /// <param name="enable">是否启用</param>
        /// <returns>服务描述符。</returns>
        public static ServiceDescriptor EnableAuthorization(this ServiceDescriptor descriptor, bool enable)
        {
            descriptor.Metadatas["EnableAuthorization"] = enable;
            return descriptor;
        }

        /// <summary>
        /// 获取是否启用授权
        /// </summary>
        /// <param name="descriptor">服务描述符。</param>
        /// <returns>服务描述符。</returns>
        public static bool EnableAuthorization(this ServiceDescriptor descriptor)
        {
            return descriptor.GetMetadata("EnableAuthorization", false);
        }

        public static ServiceDescriptor HttpMethod(this ServiceDescriptor descriptor, string httpMethod)
        {
            descriptor.Metadatas["HttpMethod"] = httpMethod;
            return descriptor;
        }

        public static string HttpMethod(this ServiceDescriptor descriptor)
        {
            return descriptor.GetMetadata("httpMethod", "");
        }

        /// <summary>
        /// 设置是否禁用外网访问
        /// </summary>
        /// <param name="descriptor">服务描述符。</param>
        /// <param name="enable">是否禁用</param>
        /// <returns>服务描述符。</returns>
        public static ServiceDescriptor DisableNetwork(this ServiceDescriptor descriptor, bool enable)
        {
            descriptor.Metadatas["DisableNetwork"] = enable;
            return descriptor;
        }

        /// <summary>
        /// 获取是否禁用外网访问
        /// </summary>
        /// <param name="descriptor">服务描述符。</param>
        /// <returns>服务描述符。</returns>
        public static bool DisableNetwork(this ServiceDescriptor descriptor)
        {
            return descriptor.GetMetadata("DisableNetwork", false);
        }

        /// <summary>
        /// 获取授权类型
        /// </summary>
        /// <param name="descriptor">服务描述符。</param>
        /// <returns>服务描述符。</returns>
        public static string AuthType(this ServiceDescriptor descriptor)
        {
            return descriptor.GetMetadata("AuthType", "");
        }


        /// <summary>
        /// 设置授权类型
        /// </summary>
        /// <param name="descriptor">服务描述符。</param>
        /// <param name="authType">授权类型</param>
        /// <returns>服务描述符。</returns>
        public static ServiceDescriptor AuthType(this ServiceDescriptor descriptor, AuthorizationType authType)
        {
            descriptor.Metadatas["AuthType"] = authType.ToString();
            return descriptor;
        }

        /// <summary>
        /// 获取负责人
        /// </summary>
        /// <param name="descriptor">服务描述符。</param>
        /// <returns></returns>
        public static string Director(this ServiceDescriptor descriptor)
        {
            return descriptor.GetMetadata<string>("Director");
        }

        /// <summary>
        /// 获取日期
        /// </summary>
        /// <param name="descriptor">服务描述符。</param>
        /// <param name="date">日期</param>
        /// <returns>服务描述符。</returns>
        public static ServiceDescriptor Date(this ServiceDescriptor descriptor, string date)
        {
            descriptor.Metadatas["Date"] = date;
            return descriptor;
        }

        /// <summary>
        /// 获取日期
        /// </summary>
        /// <param name="descriptor">服务描述符。</param>
        /// <returns>服务描述符。</returns>
        public static string Date(this ServiceDescriptor descriptor)
        {
            return descriptor.GetMetadata<string>("Date");
        }

        /// <summary>
        /// 获取释放等待执行的设置。
        /// </summary>
        /// <param name="descriptor">服务描述符。</param>
        /// <returns>如果需要等待执行则为true，否则为false，默认为true。</returns>
        public static bool WaitExecution(this ServiceDescriptor descriptor)
        {
            return descriptor.GetMetadata("WaitExecution", true);
        }
    }
}
