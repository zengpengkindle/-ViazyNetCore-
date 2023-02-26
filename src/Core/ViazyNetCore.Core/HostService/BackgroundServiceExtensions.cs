using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// 表示后台服务的扩展库。
    /// </summary>
    public static class BackgroundServiceExtensions
    {
        /// <summary>
        /// 启动后台服务。
        /// </summary>
        /// <param name="backgroundService">后台服务。</param>
        /// <param name="stoppingToken">取消标记。</param>
        /// <returns>异步任务。</returns>
        public static async Task RunAsync(this IScheduleBackgroundService backgroundService, CancellationToken stoppingToken)
        {
            var errorCount = 0;
            while(!stoppingToken.IsCancellationRequested)
            {
                var interval = TimeSpan.Zero;
                using(backgroundService.Logger.BeginScope(backgroundService.Name))
                {
                    var beginTime = DateTime.Now;
                    var state = new ScheduleServiceState();
                    try
                    {
                        backgroundService.Logger.LogInformation("The background service '{0}' running {1}.", backgroundService.Name, beginTime);
                        var task = backgroundService.ExecuteAsync(state);
                        if(backgroundService.Timeout != Timeout.InfiniteTimeSpan)
                        {
                            task = task.TimeoutAfter(backgroundService.Timeout);
                        }
                        interval = await task;
                        if(interval < TimeSpan.Zero)
                            throw new NotSupportedException($"Invalid result '{interval}' on run next time.");
                    }
                    catch(TimeoutException ex)
                    {
                        var endTime = DateTime.Now;
                        state.IsTimeout = true;
                        //- 【{0}】任务执行严重超时 {1}，本次运行累计发生错误 {2} 次
                        backgroundService.Logger.LogCritical(ex, "The background service '{0}' timeout {1}, total this error {2} times.use time {3}s."
                            , backgroundService.Name
                            , backgroundService.Timeout
                            , ++errorCount
                            , Math.Round((endTime - beginTime).TotalSeconds));
                        interval = TimeSpan.FromSeconds(5); //- 只要是发生错误，一律 5 秒后重试
                    }
                    catch(Exception ex)
                    {
                        var endTime = DateTime.Now;
                        //- 【{0}】任务执行发生错误，本次运行累计发生错误 {1} 次
                        backgroundService.Logger.LogError(ex, "The background service '{0}' error, total this error {1} times.use time {2}s."
                            , backgroundService.Name
                            , ++errorCount
                            , Math.Round((endTime - beginTime).TotalSeconds));
                        interval = TimeSpan.FromSeconds(5); //- 只要是发生错误，一律 5 秒后重试
                    }
                    finally
                    {
                        var endTime = DateTime.Now;
                        //- 【{0}】结束执行任务，用时 {1}ms，下一次任务执行时间：{2}
                        backgroundService.Logger.LogInformation("The background service '{0}' runned use {1}s, next run on {2} time."
                            , backgroundService.Name
                            , Math.Round((endTime - beginTime).TotalSeconds, 3)
                            , endTime.Add(interval).ToLongTimeString());
                    }

                    if(interval == TimeSpan.Zero)
                    {
                        //- 【{0}】任务被执行者强制终止
                        backgroundService.Logger.LogWarning("he background service '{0}' is forced stoped.", backgroundService.Name);
                        break;
                    }
                }
                await Task.Delay(interval, stoppingToken);
            }
        }
    }
}
