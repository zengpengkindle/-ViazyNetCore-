using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Quartz.Spi;
using Quartz;
using ViazyNetCore.TaskScheduler;
using Microsoft.AspNetCore.Builder;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class JobSetup
    {
        public static void AddJobSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddSingleton<ISchedulerCenter, SchedulerCenterServer>();
            services.AddScoped<TaskService>();
            services.AddScoped<TasksLogService>();
        }

        /// <summary>
        /// 程序启动后添加任务计划
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseJobSchedulers(this IApplicationBuilder app)
        {
            TaskService tasksQzService = app.ApplicationServices.GetRequiredService<TaskService>();
            ISchedulerCenter schedulerCenter = app.ApplicationServices.GetRequiredService<ISchedulerCenter>();

            var tasks = tasksQzService.GetAllStart().ConfigureAwait(false).GetAwaiter().GetResult();

            //程序启动后注册所有定时任务
            foreach (var task in tasks)
            {
                schedulerCenter.AddScheduleJobAsync(task);
            }

            return app;
        }
    }
}
