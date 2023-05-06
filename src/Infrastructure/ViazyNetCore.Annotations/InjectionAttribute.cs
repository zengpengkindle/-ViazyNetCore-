using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ViazyNetCore
{
    /// <summary>
    /// 表示一个依赖注入的特征。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class InjectionAttribute : Attribute, IInjectionAttribute
    {
        /// <summary>
        /// 获取一个值，表示服务类型列表。
        /// </summary>
        /// <value>
        /// <para>该属性为空数组时，表示服务类型和实现类型一致。</para></value>
        public Type[] ServiceTypes { get; }

        /// <summary>
        /// 获取或设置一个值，表示是否允许多个服务。
        /// </summary>
        public bool AllowMultiple { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示依赖注入的生命周期。
        /// </summary>
        public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Scoped;

        /// <summary>
        /// 初始化一个 <see cref="InjectionAttribute"/> 类的新实例。
        /// </summary>
        /// <param name="types">服务类型列表。</param>
        public InjectionAttribute(params Type[]? types)
        {
            this.ServiceTypes = types ?? Array.Empty<Type>();
        }

        /// <summary>
        /// 初始化一个 <see cref="InjectionAttribute"/> 类的新实例。
        /// </summary>
        /// <param name="types">服务类型列表。</param>
        public InjectionAttribute()
        {
            this.ServiceTypes = Array.Empty<Type>();
        }
    }
}
