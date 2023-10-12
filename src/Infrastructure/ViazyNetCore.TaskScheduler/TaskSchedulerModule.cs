using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ViazyNetCore.TaskScheduler
{
    public class TaskSchedulerModule : InjectionModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddJobSetup();
        }
    }
}
