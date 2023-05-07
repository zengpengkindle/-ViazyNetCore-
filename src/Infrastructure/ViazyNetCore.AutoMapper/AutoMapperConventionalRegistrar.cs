using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using ViazyNetCore.DependencyInjection;

namespace ViazyNetCore.AutoMapper
{
    public class AutoMapperConventionalRegistrar : DefaultConventionalRegistrar
    {
        protected readonly Type[] OpenTypes = {
            typeof(IValueResolver<,,>),
            typeof(IMemberValueResolver<,,,>),
            typeof(ITypeConverter<,>),
            typeof(IValueConverter<,>),
            typeof(IMappingAction<,>)
        };

        protected override bool IsConventionalRegistrationDisabled(Type type)
        {
            return !type.GetInterfaces().Any(x => x.IsGenericType && OpenTypes.Contains(x.GetGenericTypeDefinition())) ||
                   base.IsConventionalRegistrationDisabled(type);
        }

        protected override ServiceLifetime? GetDefaultLifeTimeOrNull(Type type)
        {
            return ServiceLifetime.Transient;
        }
    }

}
