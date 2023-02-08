using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using ViazyNetCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {

        private const string DEFAULT_PREFIX = "Default";
        /// <summary>
        /// 依赖注入指定程序集所有指定 <see cref="InjectionAttribute"/> 特征的类型。
        /// </summary>
        /// <param name="services">服务集合。</param>
        /// <param name="assemblies">程序集的列表。</param>
        /// <returns>服务集合。</returns>
        public static IServiceCollection AddAssemblyServices(this IServiceCollection services, ServiceLifetime? serviceLifetime = null, params Assembly?[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                if (assembly == null) continue;
                foreach (var t in assembly.DefinedTypes)
                {
                    RegisterInjectionService(services, t, serviceLifetime);
                }
            }
            return services;
        }

        private static string GetServiceName(string name)
        {
            if (name.iStartsWith(DEFAULT_PREFIX)) name = name[DEFAULT_PREFIX.Length..];
            return string.Concat("I", name);
        }

        private static IEnumerable<Type> GetServiceTypes(InjectionAttribute attr, params Type?[] oriTypes)
        {
            if (attr.ServiceTypes.Length > 0)
            {
                foreach (var type in attr.ServiceTypes)
                {
                    yield return type;
                }
            }
            else
            {
                foreach (var oriType in oriTypes)
                {
                    if (oriType is not null) yield return oriType;
                }
            }
        }
        private static void RegisterInjectionService(this IServiceCollection services, TypeInfo t, ServiceLifetime? serviceLifetime = null)
        {
            if (t.IsAbstract || t.IsInterface || t.IsAnonymous()) return;

            var iattr = t.GetAttribute<IInjectionAttribute>();
            var interfaceType = iattr is null ? t.GetInterface(GetServiceName(t.Name)) : null;

            if (iattr is null && interfaceType is not null) iattr = interfaceType.GetAttribute<IInjectionAttribute>();
            if (iattr is null) return;

            if (iattr is InjectionAttribute attr)
            {
                Action<ServiceDescriptor> invokeMethod = services.TryAdd;
                if (attr.AllowMultiple) invokeMethod = services.Add;

                foreach (var serviceType in GetServiceTypes(attr, interfaceType, t))
                {
                    if (typeof(IHostedService).IsAssignableFrom(t))
                        _ = services.AddTransient(typeof(IHostedService), t);
                    else if (serviceType != null)
                        invokeMethod(ServiceDescriptor.Describe(serviceType, t, serviceLifetime ?? attr.Lifetime));
                    else
                        invokeMethod(ServiceDescriptor.Describe(t, t, serviceLifetime ?? attr.Lifetime));
                }
            }
            else
            {
                throw new NotSupportedException($"The type '{iattr.GetType().FullName}' attribute is not supported.");
            }
        }

    }
}