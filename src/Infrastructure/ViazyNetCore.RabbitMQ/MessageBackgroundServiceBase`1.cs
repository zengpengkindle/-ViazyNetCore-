using Microsoft.Extensions.Logging;

using System;
using System.MQueue;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// 表示一个消息后台服务的基类。
    /// </summary>
    /// <typeparam name="TBody">实体的数据类型。</typeparam>
    public abstract class MessageBackgroundServiceBase<TBody> : ScheduleBackgroundServiceBase
    {
        /// <summary>
        /// 初始化一个 <see cref="MessageBackgroundServiceBase{TBody}"/> 类的新实例。
        /// </summary>
        /// <param name="bus">消息总线。</param>
        /// <param name="logger">服务的日志。</param>
        protected MessageBackgroundServiceBase(IMessageBus bus, ILogger logger) : base(logger)
        {
            this.Bus = bus ?? throw new ArgumentNullException(nameof(bus));
        }

        /// <summary>
        /// 获取消息总线。
        /// </summary>
        public IMessageBus Bus { get; }

        /// <inheritdoc />
        public override TimeSpan Timeout => System.Threading.Timeout.InfiniteTimeSpan;

        /// <summary>
        /// 获取或设置一个值，表示订阅选项。
        /// </summary>
        public abstract SubscribeOptions SubscribeOptions { get; }

        /// <inheritdoc />
        protected override async Task<TimeSpan> OnExecuteAsync(ScheduleServiceState state)
        {
            using(var context = this.Bus.Context)
            {
                await context.SubscribeAsync<TBody>((ss, ee) => this.OnSubscribe(context, ee)
                , this.SubscribeOptions
                , cancellationToken: state.CancellationToken);
            }
            return TimeSpan.FromSeconds(10);
        }

        /// <summary>
        /// 消息订阅。
        /// </summary>
        /// <param name="context">消息总线上下文。</param>
        /// <param name="args">订阅参数。</param>
        /// <returns>异步任务。</returns>
        protected abstract Task OnSubscribe(IMessageBusContext context, SubscribeEventArgs<TBody> args);
    }
}
