using System.Threading;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 表示异步任务的扩展库。
    /// </summary>
    public static class XTaskExtensions
    {
        /// <summary>
        /// 设定超时任务，一旦超时会抛出错误。请注意，若原任务依然会正常进行，直至执行结束，超时并不会影响原任务。
        /// </summary>
        /// <param name="task">任务。</param>
        /// <param name="timeout">超时时间。</param>
        /// <returns>任务。</returns>
        public static async Task TimeoutAfter(this Task task, TimeSpan timeout)
        {
            if(task != await Task.WhenAny(task, Task.Delay(timeout))) throw new TimeoutException();
        }

        /// <summary>
        /// 设定超时任务，一旦超时会抛出错误。请注意，若原任务依然会正常进行，直至执行结束，超时并不会影响原任务。
        /// </summary>
        /// <typeparam name="TResult">结果返回值。</typeparam>
        /// <param name="task">任务。</param>
        /// <param name="timeout">超时时间。</param>
        /// <returns>任务。</returns>
        public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout)
        {
            if(task != await Task.WhenAny(task, Task.Delay(timeout))) throw new TimeoutException();
            return task.Result;
        }

        /// <summary>
        /// 在新的环境中执行任务。
        /// </summary>
        /// <param name="task">执行的任务。</param>
        /// <param name="creationOptions">任务创建配置。</param>
        /// <param name="cancellationToken">取消标记。</param>
        /// <returns>The <paramref name="task"/> instance.</returns>
        public static void RunNew<T>(this T task, TaskCreationOptions creationOptions = TaskCreationOptions.LongRunning, CancellationToken cancellationToken = default) where T : Task
        {
            Task.Factory.StartNew(OnRunAsync, task, cancellationToken, creationOptions, TaskScheduler.Default);
        }

        private static async Task OnRunAsync(object? state)
        {
            if(state is Task task) await task;
        }

        /// <summary>
        /// 检查返回结果是否符合预期。如果受影响的行数不是为 <paramref name="result"/> 将会抛出错误。
        /// </summary>
        /// <param name="task">异步任务。</param>
        /// <param name="result">结果。</param>
        /// <returns>异步任务。</returns>
        public static async Task CheckResultAsync<T>(this Task<T> task, T result)
        {
            var value = await task.ConfigureAwait(false);
            if(Equals(value, result)) return;
            throw new InvalidCastException($"The actual execution result {value} value does not match expected result {result} value.");
        }
    }
}
