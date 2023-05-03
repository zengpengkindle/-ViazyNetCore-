using System;
using System.Collections.Generic;
using System.Linq;
using System.MQueue;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using ViazyNetCore.TaskScheduler;

namespace ViazyNetCore.Manage.WebApi.Tasks
{
    public class Job_Test : JobBase
    {
        private readonly IMessageBus _bus;

        public Job_Test(IServiceProvider serviceProvider, IMessageBus bus) : base(serviceProvider)
        {
            this._bus = bus;
        }

        protected override async Task ExecuteJob(IJobExecutionContext context)
        {
            await this._bus.PublishAsync(new MqTestModel());
            await Task.Delay(FastRandom.Instance.Next(10000));
            Console.Out.WriteLine("任务执行！");
        }
    }
}
