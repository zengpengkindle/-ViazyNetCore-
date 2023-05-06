using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ViazyNetCore.DependencyInjection
{
    public abstract class ConventionalRegistrarBase : IConventionalRegistrar
    {
        public virtual void AddAssembly(IServiceCollection services, Assembly assembly)
        {
            var types = RuntimeHelper
                .GetTypesByAssembly(assembly)
                .Where(
                    type => type != null &&
                            type.IsClass &&
                            !type.IsAbstract &&
                            !type.IsGenericType
                ).ToArray();

            AddTypes(services, types);
        }

        protected virtual bool IsConventionalRegistrationDisabled(Type type)
        {
            return type.IsDefined(typeof(DisableConventionalRegistrationAttribute), true);
        }

        public virtual void AddTypes(IServiceCollection services, params Type[] types)
        {
            foreach (var type in types)
            {
                AddType(services, type);
            }
        }

        public abstract void AddType(IServiceCollection services, Type type);

        //protected virtual bool IsConventionalRegistrationDisabled(Type type)
        //{
        //    return type.IsDefined(typeof(DisableConventionalRegistrationAttribute), true);
        //}

        protected virtual void TriggerServiceExposing(IServiceCollection services, Type implementationType, List<Type> serviceTypes)
        {
            //var exposeActions = services.GetExposingActionList();
            //if (exposeActions.Any())
            //{
            //    var args = new OnServiceExposingContext(implementationType, serviceTypes);
            //    foreach (var action in exposeActions)
            //    {
            //        action(args);
            //    }
            //}
        }

        protected virtual DependencyAttribute GetDependencyAttributeOrNull(Type type)
        {
            return type.GetCustomAttribute<DependencyAttribute>(true);
        }

        protected virtual ServiceLifetime? GetLifeTimeOrNull(Type type, DependencyAttribute dependencyAttribute)
        {
            return dependencyAttribute?.Lifetime ?? GetServiceLifetimeFromClassHierarchy(type) ?? GetDefaultLifeTimeOrNull(type);
        }

        protected virtual ServiceLifetime? GetServiceLifetimeFromClassHierarchy(Type type)
        {
            var iattr = type.GetAttribute<InjectionAttribute>();
            if (iattr != null)
            {
                return iattr.Lifetime;
            }
            //if (typeof(ITransientDependency).GetTypeInfo().IsAssignableFrom(type))
            //{
            //    return ServiceLifetime.Transient;
            //}

            //if (typeof(ISingletonDependency).GetTypeInfo().IsAssignableFrom(type))
            //{
            //    return ServiceLifetime.Singleton;
            //}

            //if (typeof(IScopedDependency).GetTypeInfo().IsAssignableFrom(type))
            //{
            //    return ServiceLifetime.Scoped;
            //}

            return null;
        }

        protected virtual ServiceLifetime? GetDefaultLifeTimeOrNull(Type type)
        {
            return null;
        }

        protected virtual List<Type> GetExposedServiceTypes(Type type)
        {
            return ExposedServiceExplorer.GetExposedServices(type);
        }

        protected virtual ServiceDescriptor CreateServiceDescriptor(
            Type implementationType,
            Type exposingServiceType,
            List<Type> allExposingServiceTypes,
            ServiceLifetime lifeTime)
        {
            if (lifeTime.IsIn(ServiceLifetime.Singleton, ServiceLifetime.Scoped))
            {
                var redirectedType = GetRedirectedTypeOrNull(
                    implementationType,
                    exposingServiceType,
                    allExposingServiceTypes
                );

                if (redirectedType != null)
                {
                    return ServiceDescriptor.Describe(
                        exposingServiceType,
                        provider => provider.GetService(redirectedType),
                        lifeTime
                    );
                }
            }

            return ServiceDescriptor.Describe(
                exposingServiceType,
                implementationType,
                lifeTime
            );
        }

        protected virtual Type GetRedirectedTypeOrNull(
            Type implementationType,
            Type exposingServiceType,
            List<Type> allExposingServiceTypes)
        {
            if (allExposingServiceTypes.Count < 2)
            {
                return null;
            }

            if (exposingServiceType == implementationType)
            {
                return null;
            }

            if (allExposingServiceTypes.Contains(implementationType))
            {
                return implementationType;
            }

            return allExposingServiceTypes.FirstOrDefault(
                t => t != exposingServiceType && exposingServiceType.IsAssignableFrom(t)
            );
        }

    }

}
