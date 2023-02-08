using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 定义复制的策略。
    /// </summary>
    public enum CopyToStrategy
    {
        /// <summary>
        /// 默认方式。
        /// </summary>
        Default,
        /// <summary>
        /// 仅限主键方式。
        /// </summary>
        OnlyPrimaryKey,
        /// <summary>
        /// 仅限非主键方式。
        /// </summary>
        ExcludePrimaryKey,
        /// <summary>
        /// 仅限被修改过的值。
        /// </summary>
        OnlyChangeValues,
    }

    /// <summary>
    /// 提供一组用于实现通用操作的扩展方法。
    /// </summary>
    public static class XCommonExtensions
    {
        /// <summary>
        /// 创建指定类型的实例。
        /// </summary>
        /// <typeparam name="T">实例的类型。</typeparam>
        /// <param name="services">服务提供程序。</param>
        /// <param name="parameters">构造函数的参数。</param>
        /// <returns>指定类型的实例。</returns>
        public static T CreateInstance<T>(this IServiceProvider services, params object[] parameters) => (T)services.CreateInstance(typeof(T), parameters);

        /// <summary>
        /// 创建指定类型的实例。
        /// </summary>
        /// <param name="services">服务提供程序。</param>
        /// <param name="instanceType">实例的类型。</param>
        /// <param name="parameters">构造函数的参数。</param>
        /// <returns>指定类型的实例。</returns>
        public static object CreateInstance(this IServiceProvider services, Type instanceType, params object[] parameters)
        {
            //return ActivatorUtilities.CreateInstance(services, instanceType, parameters);
            var typeInfo = instanceType.GetTypeInfo();
            if (!typeInfo.IsAbstract)
            {
                var ctor = typeInfo.DeclaredConstructors.FirstOrDefault(c => !c.IsStatic && c.IsPublic);
                if (ctor != null)
                {
                    var parameterInfos = ctor.GetParameters();
                    var parameterValues = new object[parameterInfos.Length];
                    for (int i = 0; i < parameterInfos.Length; i++)
                    {
                        var pd = parameterInfos[i];
                        var value = parameters.FirstOrDefault(p => pd.ParameterType.IsInstanceOfType(p));
                        if (value is null)
                        {
                            if (pd.HasDefaultValue) value = services.GetService(pd.ParameterType) ?? pd.DefaultValue;
                            else value = services.GetService(pd.ParameterType);
                        }
                        parameterValues[i] = value;
                    }

                    return ctor.CreateConstructorHandler()(parameterValues);
                }

            }

            var message = $"A suitable constructor for type '{instanceType}' could not be located. Ensure the type is concrete and services are registered for all parameters of a public constructor.";
            throw new InvalidOperationException(message);
        }

        /// <summary>
        /// 将所有字节写入流中。
        /// </summary>
        /// <param name="stream">流。</param>
        /// <param name="bytes">字节。</param>
        public static void WriteBytes(this IO.Stream stream, byte[] bytes)
        {
            stream.Write(bytes, 0, bytes.Length);
        }


        /// <summary>
        /// 指定当前内容绝对不能出现 <see langword="null"/> 值。
        /// </summary>
        /// <typeparam name="T">值的数据类型。</typeparam>
        /// <param name="t">内容。</param>
        /// <exception cref="InvalidOperationException">当 <paramref name="t"/> 为 <see langword="null"/> 值引发的错误。</exception>
        /// <returns>内容。</returns>
        public static T MustBe<T>(this T? t)
        {
            if (t is null) throw new InvalidOperationException($"The content '{typeof(T).FullName}' is must be not null value.");
            return t;
        }

        /// <summary>
        /// 指定当前内容必须是 <typeparamref name="T"/> 类型。
        /// </summary>
        /// <typeparam name="T">值的数据类型。</typeparam>
        /// <param name="obj">内容。</param>
        /// <exception cref="InvalidCastException">当 <paramref name="obj"/> 不为 <typeparamref name="T"/> 类型引发的错误。</exception>
        /// <returns>一个<typeparamref name="T"/> 类型的值。</returns>
        public static T MustBe<T>(this object? obj)
        {
            if (obj is T t) return t;
            throw new InvalidCastException($"Cannot convert from object to '{typeof(T).FullName}' type.");
        }

        ///// <summary>
        ///// 安全锁定指定种子。
        ///// </summary>
        ///// <param name="lockProvider">锁提供程序。</param>
        ///// <param name="key">生成锁对象实例的种子，将采用默认的 <see cref="EqualityComparer{String}"/> 匹配种子。</param>
        ///// <param name="execute">执行委托。</param>
        ///// <returns>异步任务。</returns>
        //public static async Task LockAsync(this Aoite.ILockProvider lockProvider, string key, Func<Task> execute)
        //{
        //    Exception exception = null;
        //    using(lockProvider.Lock(key))
        //    {
        //        try
        //        {
        //            await execute();
        //        }
        //        catch(Exception ex)
        //        {
        //            exception = ex;
        //        }
        //    }
        //    if(exception != null) throw exception;
        //}

        ///// <summary>
        ///// 安全锁定指定种子。
        ///// </summary>
        ///// <typeparam name="TResult">结果的数据类型。</typeparam>
        ///// <param name="lockProvider">锁提供程序。</param>
        ///// <param name="key">生成锁对象实例的种子，将采用默认的 <see cref="EqualityComparer{String}"/> 匹配种子。</param>
        ///// <param name="execute">执行委托。</param>
        ///// <returns>异步任务。</returns>
        //public static async Task<TResult> LockAsync<TResult>(this Aoite.ILockProvider lockProvider, string key, Func<Task<TResult>> execute)
        //{
        //    Exception exception = null;
        //    using(lockProvider.Lock(key))
        //    {
        //        try
        //        {
        //            return await execute();
        //        }
        //        catch(Exception ex)
        //        {
        //            exception = ex;
        //        }
        //    }
        //    throw exception;
        //}

        ///// <summary>
        ///// 安全锁定指定种子。
        ///// </summary>
        ///// <param name="lockProvider">锁提供程序。</param>
        ///// <param name="key">生成锁对象实例的种子，将采用默认的 <see cref="EqualityComparer{String}"/> 匹配种子。</param>
        ///// <param name="execute">执行委托。</param>
        //public static void Lock(this Aoite.ILockProvider lockProvider, string key, Action execute)
        //{
        //    Exception exception = null;
        //    using(lockProvider.Lock(key))
        //    {
        //        try
        //        {
        //            execute();
        //        }
        //        catch(Exception ex)
        //        {
        //            exception = ex;
        //        }
        //    }
        //    if(exception != null) throw exception;
        //}

        ///// <summary>
        ///// 安全锁定指定种子。
        ///// </summary>
        ///// <typeparam name="TResult">结果的数据类型。</typeparam>
        ///// <param name="lockProvider">锁提供程序。</param>
        ///// <param name="key">生成锁对象实例的种子，将采用默认的 <see cref="EqualityComparer{String}"/> 匹配种子。</param>
        ///// <param name="execute">执行委托。</param>
        ///// <returns>执行结果。</returns>
        //public static TResult Lock<TResult>(this Aoite.ILockProvider lockProvider, string key, Func<TResult> execute)
        //{
        //    Exception exception = null;
        //    using(lockProvider.Lock(key))
        //    {
        //        try
        //        {
        //            execute();
        //        }
        //        catch(Exception ex)
        //        {
        //            exception = ex;
        //        }
        //    }
        //    throw exception;
        //}

        ///// <summary>
        ///// 创建基于实体类型的锁提供程序。
        ///// </summary>
        ///// <typeparam name="TModel">实体的数据类型。</typeparam>
        ///// <param name="lockProvider">锁提供程序。</param>
        ///// <returns>一个基于类型的锁。</returns>
        //public static Aoite.ILockProvider TypeOf<TModel>(this Aoite.ILockProvider lockProvider)
        //{
        //    return new Aoite.TypeLockProvider(typeof(TModel), lockProvider);
        //}

        ///// <summary>
        ///// 以安全的方式执行代码，并释放当前对象。
        ///// </summary>
        ///// <typeparam name="T">方法执行后要释放对象的数据类型。</typeparam>
        ///// <param name="disposable">可释放对象。</param>
        ///// <param name="execute">执行方法。</param>
        //public static void Using<T>(this T disposable, Action<T> execute) where T : IDisposable
        //{
        //    if(disposable is null) throw new ArgumentNullException(nameof(disposable));
        //    if(execute is null) throw new ArgumentNullException(nameof(execute));

        //    try
        //    {
        //        execute(disposable);
        //    }
        //    finally
        //    {
        //        disposable.Dispose();
        //    }
        //}

        ///// <summary>
        ///// 以安全的方式执行代码返回结果，并释放当前对象。
        ///// </summary>
        ///// <typeparam name="T">方法执行后要释放对象的数据类型。</typeparam>
        ///// <typeparam name="TResult">结果的数据类型。</typeparam>
        ///// <param name="disposable">可释放对象。</param>
        ///// <param name="execute">执行方法。</param>
        ///// <returns>执行无异常时的结果。</returns>
        //public static TResult Using<T, TResult>(this T disposable, Func<T, TResult> execute) where T : IDisposable
        //{
        //    if(disposable is null) throw new ArgumentNullException(nameof(disposable));
        //    if(execute is null) throw new ArgumentNullException(nameof(execute));

        //    try
        //    {
        //        return execute(disposable);
        //    }
        //    finally
        //    {
        //        disposable.Dispose();
        //    }

        //}

        ///// <summary>
        ///// 以安全的方式执行异步代码，并释放当前对象。
        ///// </summary>
        ///// <typeparam name="T">方法执行后要释放对象的数据类型。</typeparam>
        ///// <param name="disposable">可释放对象。</param>
        ///// <param name="execute">执行方法。</param>
        ///// <returns>异步任务。</returns>
        //public static async Task Using<T>(this T disposable, Func<T, Task> execute) where T : IDisposable
        //{
        //    if(disposable is null) throw new ArgumentNullException(nameof(disposable));
        //    if(execute is null) throw new ArgumentNullException(nameof(execute));
        //    try
        //    {
        //        await execute(disposable);
        //    }
        //    finally
        //    {
        //        disposable.Dispose();
        //    }
        //}

        ///// <summary>
        ///// 以安全的方式执行异步代码返回结果，并释放当前对象。
        ///// </summary>
        ///// <typeparam name="T">方法执行后要释放对象的数据类型。</typeparam>
        ///// <typeparam name="TResult">结果的数据类型。</typeparam>
        ///// <param name="disposable">可释放对象。</param>
        ///// <param name="execute">执行方法。</param>
        ///// <returns>异步任务。</returns>
        //public static async Task<TResult> Using<T, TResult>(this T disposable, Func<T, Task<TResult>> execute) where T : IDisposable
        //{
        //    if(disposable is null) throw new ArgumentNullException(nameof(disposable));
        //    if(execute is null) throw new ArgumentNullException(nameof(execute));
        //    try
        //    {
        //        return await execute(disposable);
        //    }
        //    finally
        //    {
        //        disposable.Dispose();
        //    }
        //}

        /// <summary>
        /// 将当前对象和更多对象的属性合并成新的对象。
        /// </summary>
        /// <typeparam name="TSource">合并源的数据类型。</typeparam>
        /// <param name="source">合并源的对象。</param>
        /// <returns>一个合并源的方法。</returns>
        public static IMergeSource<TSource> Extend<TSource>(this TSource source) => MergerData.From(source);

        /// <summary>
        /// 将当前时间转换为 Javascript Time 值。同等于 JS 代码：“(new Date()).getTime()”
        /// </summary>
        /// <param name="dateTime">时间。</param>
        /// <returns>Javascript Time 值。</returns>
        public static long ConvertToJsTime(this DateTime dateTime) => (dateTime.ToUniversalTime().Ticks - 621355968000000000L) / 10000L;

        /// <summary>
        /// 将当前 Javascript Time 值转换为时间。
        /// </summary>
        /// <param name="time">Javascript Time 值</param>
        /// <returns>时间。</returns>
        public static DateTime ConvertFromJsTime(this long time) => new DateTime(time * 10000 + 621355968000000000L, DateTimeKind.Utc).ToLocalTime();

        /// <summary>
        /// 判断时间戳是否在有效期内。
        /// </summary>
        /// <param name="timestamp">时间戳。</param>
        /// <param name="expiresIn">有效期的秒数。</param>
        /// <returns>数据有效返回 true，否则返回 false。</returns>
        public static bool IsValid(this DateTime timestamp, double expiresIn) => timestamp.AddSeconds(expiresIn + 1.0) >= DateTime.Now;

        /// <summary>
        /// 将当前对象和另一个对象的属性合并成新的对象。
        /// </summary>
        /// <typeparam name="TSource">合并源的数据类型。</typeparam>
        /// <typeparam name="TTarget">合并目标的数据类型。</typeparam>
        /// <param name="source">合并源的对象。</param>
        /// <param name="target">合并目标的对象。</param>
        /// <returns>一个新的对象，包含源和目标的所有属性和值。</returns>
        public static object Extend<TSource, TTarget>(this TSource source, TTarget target) => MergerData.From(source).With(target).Build();

        /// <summary>
        /// 将 <paramref name="target"/> 所有的属性值复制到当前对象。
        /// </summary>
        /// <typeparam name="TSource">源的数据类型。</typeparam>
        /// <typeparam name="TTarget">目标的数据类型。</typeparam>
        /// <param name="source">复制的源对象。</param>
        /// <param name="target">复制的目标对象。</param>
        /// <param name="targetStrategy">复制目标的策略。</param>
        /// <returns> <paramref name="source"/>。</returns>
        public static TSource CopyFrom<TSource, TTarget>(this TSource source, TTarget target, CopyToStrategy targetStrategy = CopyToStrategy.Default)
            => CopyTo(target, source, targetStrategy);

        /// <summary>
        /// 将当前对象所有的属性值复制成一个新的 <typeparamref name="TTarget"/> 实例。
        /// </summary>
        /// <typeparam name="TTarget">新的数据类型。</typeparam>
        /// <param name="source">复制的源对象。</param>
        /// <param name="targetStrategy">复制目标的策略。</param>
        /// <returns><typeparamref name="TTarget"/> 的新实例。</returns>
        public static TTarget CopyTo<TTarget>(this object source, CopyToStrategy targetStrategy = CopyToStrategy.Default)
        {
            if (source is null) return default;
            TTarget t2 = Activator.CreateInstance<TTarget>();
            return CopyTo(source, t2, targetStrategy);
        }

        /// <summary>
        /// 将当前对象所有的属性值复制到 <paramref name="target"/>。
        /// </summary>
        /// <typeparam name="TTarget">目标的数据类型。</typeparam>
        /// <param name="source">复制的源对象。</param>
        /// <param name="target">复制的目标对象。</param>
        /// <param name="targetStrategy">复制目标的策略。</param>
        /// <returns> <paramref name="target"/>。</returns>
        public static TTarget CopyTo<TTarget>(this object source, TTarget target, CopyToStrategy targetStrategy = CopyToStrategy.Default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (Equals(target, default(TTarget))) throw new ArgumentNullException(nameof(target));

            var sMapper = TypeMapper.Create(source.GetType());
            var tMapper = TypeMapper.Create(target.GetType());
            foreach (var sProperty in sMapper.Properties)
            {
                var tProperty = tMapper[sProperty.Name];
                if (tProperty is null) continue;
                if (targetStrategy == CopyToStrategy.OnlyPrimaryKey && !tProperty.IsKey
                    || (targetStrategy == CopyToStrategy.ExcludePrimaryKey && tProperty.IsKey)
                    || !tProperty.Property.CanWrite) continue;

                object sValue = sProperty.GetValue(source);

                if (targetStrategy == CopyToStrategy.OnlyChangeValues) if (Equals(sValue, sProperty.TypeDefaultValue)) continue;

                var spType = sProperty.Property.PropertyType;
                var tpType = tProperty.Property.PropertyType;

                if (spType != tpType)
                {
                    if (tpType.IsValueType && sValue is null) continue;
                    tProperty.SetValue(target, sValue.CastTo(tpType));
                }
                else tProperty.SetValue(target, sValue);
            }
            return target;
        }

        /// <summary>
        /// 将当前对象所有的属性值复制成一个新的 <typeparamref name="TTarget"/> 实例。
        /// </summary>
        /// <typeparam name="TTarget">目标的数据类型。</typeparam>
        /// <param name="source">复制的源对象。</param>
        /// <param name="targetStrategy">复制目标的策略。</param>
        /// <returns><typeparamref name="TTarget"/> 的新实例。</returns>
        public static TTarget DeepCopy<TTarget>(this TTarget source, CopyToStrategy targetStrategy = CopyToStrategy.Default)
        {
            return source.CopyTo<TTarget>(targetStrategy);
        }

        /// <summary>
        /// 尝试将给定值转换为指定的数据类型。
        /// </summary>
        /// <typeparam name="T">要转换的数据类型。</typeparam>
        /// <param name="value">请求类型转换的值。</param>
        /// <returns>将 <paramref name="value"/> 转换为 <typeparamref name="T"/> 的值。</returns>
        public static T CastTo<T>(this object value) => (T)CastTo(value, typeof(T));

        /// <summary>
        /// 尝试将给定值转换为指定的数据类型。
        /// </summary>
        /// <param name="value">请求类型转换的值。</param>
        /// <param name="type">要转换的数据类型。</param>
        /// <returns>将 <paramref name="value"/> 转换为 <paramref name="type"/> 的值。</returns>
        public static object CastTo(this object value, Type type) => type.Parse(value);

        /// <summary>
        /// 尝试释放当前对象使用的所有资源
        /// </summary>
        /// <param name="obj">释放的对象。</param>
        public static void TryDispose(this IDisposable obj)
        {
            if (obj != null) obj.Dispose();
        }

        /// <summary>
        /// 将指定的金额转换为中文表示。
        /// </summary>
        /// <param name="money">数字表示的金额。</param>
        /// <returns>中文表示的金额。</returns>
        public static string ToChinese(this decimal money)
        {
            var s = money.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            var d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            return Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟万亿兆京垓秭穰"[m.Value[0] - '-'].ToString());
        }

        /// <summary>
        /// 读取所有流的所有字节。
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="encoding">编号。</param>
        /// <returns>所有字节。</returns>
        public static async Task<string> ReadToEndAsync(this IO.Stream stream, Encoding encoding = null)
        {
            using var reader = encoding is null ? new IO.StreamReader(stream
                 , detectEncodingFromByteOrderMarks: false
                 , leaveOpen: true) : new IO.StreamReader(stream, encoding
                 , detectEncodingFromByteOrderMarks: false
                 , leaveOpen: true);
            return await reader.ReadToEndAsync();
        }


        /// <summary>
        /// 读取所有流的所有字节。
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="encoding">编号。</param>
        /// <returns>所有字节。</returns>
        public static string ReadToEnd(this IO.Stream stream, Encoding encoding = null)
        {
            using var reader = encoding is null ? new IO.StreamReader(stream
                 , detectEncodingFromByteOrderMarks: false
                 , leaveOpen: true) : new IO.StreamReader(stream, encoding
                 , detectEncodingFromByteOrderMarks: false
                 , leaveOpen: true);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// 设定超时任务，一旦超时会抛出错误。请注意，若原任务依然会正常进行，直至执行结束，超时并不会影响原任务。
        /// </summary>
        /// <param name="task">任务。</param>
        /// <param name="timeout">超时时间。</param>
        /// <returns>任务。</returns>
        public static async Task TimeoutAfter(this Task task, TimeSpan timeout)
        {
            if (task != await Task.WhenAny(task, Task.Delay(timeout))) throw new TimeoutException();
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
            if (task != await Task.WhenAny(task, Task.Delay(timeout))) throw new TimeoutException();
            return task.Result;
        }

        /// <summary>
        /// 在新的环境中执行任务。
        /// </summary>
        /// <param name="task">执行的任务。</param>
        /// <param name="creationOptions">A TaskCreationOptions value that controls the behavior of the created.</param>
        /// <param name="cancellationToken">The System.Threading.Tasks.TaskFactory.CancellationToken that will be assigned to the new task.</param>
        /// <returns>The <paramref name="task"/> instance.</returns>
        public static void RunNew<T>(this T task, TaskCreationOptions creationOptions = TaskCreationOptions.LongRunning, CancellationToken cancellationToken = default) where T : Task
        {
            Task.Factory.StartNew(OnRunAsync, task, cancellationToken, creationOptions, TaskScheduler.Default);
        }

        private static async Task OnRunAsync(object state)
        {
            var task = state as Task;
            await task;
        }

        /// <summary>
        /// 指定当前内容绝对不能出现 null 值。
        /// </summary>
        /// <typeparam name="T">值的数据类型。</typeparam>
        /// <param name="t">内容。</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static T MustBe<T>(this T? t)
        {
            if (t == null)
            {
                throw new InvalidOperationException("The content '" + typeof(T).FullName + "' is must be not null value.");
            }

            return t;
        }
    }
}
