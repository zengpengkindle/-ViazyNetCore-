using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ViazyNetCore.AspNetCore;
using ViazyNetCore.Modules;

namespace ViazyNetCore.TaskScheduler
{
    [DependsOn(typeof(TaskSchedulerModule)
        , typeof(AspNetCoreModule))]
    public class TaskSchedulerJobModule : InjectionModule
    {
        public override async Task OnApplicationInitializationAsync([NotNull] ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            var tasksQzService = app.ApplicationServices.GetRequiredService<TaskService>();
            var schedulerCenter = app.ApplicationServices.GetRequiredService<ISchedulerCenter>();

            var tasks = await tasksQzService.GetAllStart();

            //程序启动后注册所有定时任务
            foreach (var task in tasks)
            {
                await schedulerCenter.AddScheduleJobAsync(task);
            }
        }
    }
}
