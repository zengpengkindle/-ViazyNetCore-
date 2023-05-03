using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Model.Models;
using FreeSql.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace ViazyNetCore.TaskScheduler
{
    public abstract class JobBase : IJob
    {
        private readonly TasksLogService? _tasksLogService;
        private readonly TaskService? _tasksQzService;

        public JobBase(IServiceProvider serviceProvider)
        {
            this._tasksQzService = serviceProvider.GetService<TaskService>();
            this._tasksLogService = serviceProvider.GetService<TasksLogService>();
        }

        protected abstract Task ExecuteJob(IJobExecutionContext context);

        /// <inheritdoc/>
        public async Task Execute(IJobExecutionContext context)
        {
            await this.ExecuteJob(context);
        }
    }
}
