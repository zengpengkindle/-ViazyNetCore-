using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// 定义一个计划性后台服务的基类。
    /// </summary>
    public interface IScheduleBackgroundService : IHostedService
    {
        /// <summary>
        /// 获取服务的名称。
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 获取服务的日志。
        /// </summary>
        ILogger Logger { get; }
        /// <summary>
        /// 获取一个值，表示默认执行超时时间。
        /// </summary>
        TimeSpan Timeout { get; }

        /// <summary>
        /// 执行任务。
        /// </summary>
        /// <param name="state">服务状态。</param>
        /// <returns>下一次执行间隔的异步任务。</returns>
        Task<TimeSpan> ExecuteAsync(ScheduleServiceState state);
    }
}
