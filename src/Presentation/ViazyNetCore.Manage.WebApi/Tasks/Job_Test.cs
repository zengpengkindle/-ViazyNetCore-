using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using ViazyNetCore.TaskScheduler;

namespace ViazyNetCore.Manage.WebApi.Tasks
{
    public class Job_Test : JobBase
    {
        public Job_Test(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Task RunJob(IJobExecutionContext context)
        {
            Console.WriteLine("任务执行！");
            return Task.CompletedTask;
        }
    }
}
