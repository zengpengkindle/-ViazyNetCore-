using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Quartz.Spi;
using Quartz;
using ViazyNetCore.TaskScheduler;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class JobSetup
    {
        public static void AddJobSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddSingleton<ISchedulerCenter, SchedulerCenterServer>();
            services.AddScoped<TasksQzService>();
            services.AddScoped<TasksLogService>();
        }
    }
}
