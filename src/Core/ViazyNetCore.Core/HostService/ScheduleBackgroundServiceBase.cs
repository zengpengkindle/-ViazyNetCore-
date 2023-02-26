using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.Hosting
{
    public abstract class ScheduleBackgroundServiceBase : BackgroundService, IScheduleBackgroundService
    {
        /// <summary>
        /// 获取服务的名称。
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// 获取服务的日志。
        /// </summary>
        public ILogger Logger { get; }

        /// <summary>
        /// 获取一个值，表示默认执行超时时间。
        /// </summary>
        public abstract TimeSpan Timeout { get; }

        /// <summary>
        /// 初始化一个 <see cref="ScheduleBackgroundServiceBase"/> 类的新实例。
        /// </summary>
        /// <param name="logger">服务的日志。</param>
        public ScheduleBackgroundServiceBase(ILogger logger)
        {
            this.Logger = logger;
        }

        /// <summary>
        /// 执行任务。
        /// </summary>
        /// <param name="state">服务状态。</param>
        /// <returns>下一次执行间隔的异步任务。</returns>
        protected abstract Task<TimeSpan> OnExecuteAsync(ScheduleServiceState state);

        /// <summary>
        /// 执行后台服务。
        /// </summary>
        /// <param name="stoppingToken">取消标记。</param>
        /// <returns>异步任务。</returns>
        protected sealed override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return this.RunAsync(stoppingToken);
        }

        Task<TimeSpan> IScheduleBackgroundService.ExecuteAsync(ScheduleServiceState state) => this.OnExecuteAsync(state);
    }
}
