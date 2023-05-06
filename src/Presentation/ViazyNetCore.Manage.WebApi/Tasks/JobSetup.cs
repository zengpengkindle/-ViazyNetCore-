using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Quartz.Spi;
using Quartz;
using ViazyNetCore.TaskScheduler;
using ViazyNetCore.Manage.WebApi.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TestJobSetup
    {
        public static void AddJobTaskSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            //注入测试任务
            services.AddTransient<Job_Test>();
            services.AddTransient<Job_RabbitMqTest>();
        }
    }
}
