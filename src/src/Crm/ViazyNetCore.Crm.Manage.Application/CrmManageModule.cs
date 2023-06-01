using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.AutoMapper;
using ViazyNetCore.Modules;

namespace ViazyNetCore.Crm.Manage.Application
{
    [DependsOn(typeof(AutoMapperModule)
        , typeof(CrmModulesModule)
        )]
    public class CrmManageModule : InjectionModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AutoMapperOptions>(options => options.AddMaps<CrmManageModule>());
        }

        public override void OnApplicationInitialization([NotNull] ApplicationInitializationContext context)
        {
        }
    }
}
