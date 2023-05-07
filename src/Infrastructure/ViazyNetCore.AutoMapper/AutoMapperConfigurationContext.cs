using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace ViazyNetCore.AutoMapper
{
    public class AutoMapperConfigurationContext : IAutoMapperConfigurationContext
    {
        public IMapperConfigurationExpression MapperConfiguration { get; }

        public IServiceProvider ServiceProvider { get; }

        public AutoMapperConfigurationContext(
            IMapperConfigurationExpression mapperConfigurationExpression,
            IServiceProvider serviceProvider)
        {
            MapperConfiguration = mapperConfigurationExpression;
            ServiceProvider = serviceProvider;
        }
    }
}
