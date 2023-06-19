using Microsoft.Extensions.Logging;
using Quartz;
using ViazyNetCore.RabbitMQ;

namespace ViazyNetCore.TaskScheduler
{
    public abstract class RabbitMqJobBase<TBody> : JobBase, IInterruptableJob
    {
        public IMessageBus Bus { get; }

        public ILogger Logger { get; }

        /// <summary>
        /// 获取或设置一个值，表示订阅选项。
        /// </summary>
        public virtual SubscribeOptions SubscribeOptions { get; } = new SubscribeOptions();

        protected RabbitMqJobBase(IMessageBus bus, ILogger logger, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.Bus = bus;
            this.Logger = logger;
        }

        protected override async Task ExecuteJob(IJobExecutionContext jobContext)
        {
            using var context = this.Bus.Context;
            await context.SubscribeAsync<TBody>((ss, ee) => this.OnSubscribe(context, ee, jobContext.CancellationToken)
            , this.SubscribeOptions
            , cancellationToken: jobContext.CancellationToken);
        }

        /// <summary>
        /// 消息订阅。
        /// </summary>
        /// <param name="context">消息总线上下文。</param>
        /// <param name="args">订阅参数。</param>
        /// <returns>异步任务。</returns>
        protected abstract Task OnSubscribe(IMessageBusContext context, SubscribeEventArgs<TBody> args, CancellationToken cancellationToken);
    }
}