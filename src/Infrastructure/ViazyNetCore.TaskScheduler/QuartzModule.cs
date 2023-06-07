using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Impl;
using ViazyNetCore.Modules;

namespace ViazyNetCore.TaskScheduler
{
    public class QuartzModule : InjectionModule
    {
        private IScheduler? _scheduler;

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var options = context.Services.ExecutePreConfiguredActions<QuartzOptions>();
            context.Services.AddQuartz(options.Properties, build =>
            {
                // these are the defaults
                if (options.Properties[StdSchedulerFactory.PropertySchedulerJobFactoryType] == null)
                {
                    build.UseMicrosoftDependencyInjectionJobFactory();
                }

                if (options.Properties[StdSchedulerFactory.PropertySchedulerTypeLoadHelperType] == null)
                {
                    build.UseSimpleTypeLoader();
                }

                if (options.Properties[StdSchedulerFactory.PropertyJobStoreType] == null)
                {
                    build.UseInMemoryStore();
                }

                if (options.Properties[StdSchedulerFactory.PropertyThreadPoolType] == null)
                {
                    build.UseDefaultThreadPool(tp =>
                    {
                        tp.MaxConcurrency = 10;
                    });
                }

                //if (options.Properties["quartz.plugin.timeZoneConverter.type"] == null)
                //{
                //    build.UseTimeZoneConverter();
                //}

                options.Configurator?.Invoke(build);
            });

            context.Services.AddSingleton(serviceProvider =>
            {
                return AsyncHelper.RunSync(() => serviceProvider.GetRequiredService<ISchedulerFactory>().GetScheduler());
            });

            this.Configure<QuartzOptions>(quartzOptions =>
            {
                quartzOptions.Properties = options.Properties;
                quartzOptions.StartDelay = options.StartDelay;
            });

        }

        public async override Task OnApplicationInitializationAsync([NotNull] ApplicationInitializationContext context)
        {
            var options = context.ServiceProvider.GetRequiredService<IOptions<QuartzOptions>>().Value;

            this._scheduler = context.ServiceProvider.GetRequiredService<IScheduler>();

            await options.StartSchedulerFactory.Invoke(_scheduler);
        }

        public async override Task OnApplicationShutdownAsync([NotNull] ApplicationShutdownContext context)
        {
            if (_scheduler!.IsStarted)
            {
                await _scheduler.Shutdown();
            }
        }

        public override void OnApplicationInitialization([NotNull] ApplicationInitializationContext context)
        {
            AsyncHelper.RunSync(() => OnApplicationInitializationAsync(context));
        }

        public override void OnApplicationShutdown([NotNull] ApplicationShutdownContext context)
        {
            AsyncHelper.RunSync(() => OnApplicationShutdownAsync(context));
        }
    }
}
