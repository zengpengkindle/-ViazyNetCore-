using FreeScheduler;
using FreeSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ViazyNetCore.TaskScheduler
{
    public static class TaskSchedulerServiceExtensions
    {
        public const string DbKey = "DbKey";

        /// <summary>
        /// 添加任务调度
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureOptions"></param>
        public static IServiceCollection AddTaskScheduler(this IServiceCollection services, Action<TaskSchedulerOptions> configureOptions = null)
        {
            return services.AddTaskScheduler(DbKey, configureOptions);
        }

        /// <summary>
        /// 添加任务调度
        /// </summary>
        /// <param name="services"></param>
        /// <param name="dbKey"></param>
        /// <param name="configureOptions"></param>
        public static IServiceCollection AddTaskScheduler(this IServiceCollection services, string dbKey, Action<TaskSchedulerOptions> configureOptions = null)
        {
            var serviceProvider = services.BuildServiceProvider();
            var freeSql = serviceProvider.GetService<IFreeSql>();
            if (freeSql == null)
            {
                throw new ArgumentNullException(nameof(freeSql));
            }
            var options = new TaskSchedulerOptions()
            {
                FreeSql = freeSql
            };
            configureOptions?.Invoke(options);

            var taskFreeSql = options.FreeSql.UseDb("task");

            taskFreeSql.CodeFirst
            .ConfigEntity<TaskInfo>(a =>
            {
                a.Name("ad_task");
                a.Property(b => b.Id).IsPrimary(true);
                a.Property(b => b.Body).StringLength(-1);
                a.Property(b => b.Interval).MapType(typeof(int));
                a.Property(b => b.IntervalArgument).StringLength(1024);
                a.Property(b => b.Status).MapType(typeof(int));
                a.Property(b => b.CreateTime).ServerTime(DateTimeKind.Local);
                a.Property(b => b.LastRunTime).ServerTime(DateTimeKind.Local);
            })
            .ConfigEntity<TaskLog>(a =>
            {
                a.Name("ad_task_log");
                a.Property(b => b.Exception).StringLength(-1);
                a.Property(b => b.Remark).StringLength(-1);
                a.Property(b => b.CreateTime).ServerTime(DateTimeKind.Local);
            });

            options.ConfigureFreeSql?.Invoke(taskFreeSql);

            if (true)
            {
                taskFreeSql.CodeFirst.SyncStructure<TaskInfo>();
                taskFreeSql.CodeFirst.SyncStructure<TaskLog>();
            }

            if (options.TaskHandler != null && options.CustomTaskHandler == null)
            {
                //开启任务
                var scheduler = new Scheduler(options.TaskHandler);
                services.AddSingleton(scheduler);
            }
            else if (options.TaskHandler != null && options.CustomTaskHandler != null)
            {
                //开启自定义任务
                var scheduler = new Scheduler(options.TaskHandler, options.CustomTaskHandler);
                services.AddSingleton(scheduler);
            }

            return services;
        }
    }
}