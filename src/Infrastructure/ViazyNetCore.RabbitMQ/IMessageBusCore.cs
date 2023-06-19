using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ViazyNetCore.RabbitMQ
{
    /// <summary>
    /// 定义一个核心的消息总线。
    /// </summary>
    public interface IMessageBusCore
    {
        /// <summary>
        /// 获取消息管理器。
        /// </summary>
        IMessageManager Manager { get; }

        /// <summary>
        /// 异步调用消息总线。
        /// </summary>
        /// <typeparam name="TTask">异步任务。</typeparam>
        /// <param name="message">消息。</param>
        /// <param name="channelAction">通道方法。</param>
        /// <param name="connectionName">连接名称。</param>
        /// <returns>异步任务。</returns>
        TTask CallAsync<TTask>(IMessage message, Func<IChannelProxy, IMessage, TTask> channelAction, string? connectionName = null) where TTask : Task;

        /// <summary>
        /// 批量发布消息。
        /// </summary>
        /// <typeparam name="TBody">实体的数据类型。</typeparam>
        /// <param name="bodies">实体列表。</param>
        /// <param name="options">发布选项。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>异步任务。</returns>
        Task BatchPublish<TBody>(IEnumerable<TBody> bodies, PublishOptions? options = null, CancellationToken cancellationToken = default);
        /// <summary>
        /// 发布消息。
        /// </summary>
        /// <typeparam name="TBody">实体的数据类型。</typeparam>
        /// <param name="body">实体。</param>
        /// <param name="options">发布选项。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>异步任务。</returns>
        Task PublishAsync<TBody>(TBody body, PublishOptions? options = null, CancellationToken cancellationToken = default);
        /// <summary>
        /// 发布消息。
        /// </summary>
        /// <typeparam name="TBody">实体的数据类型。</typeparam>
        /// <param name="body">实体。</param>
        /// <param name="options">发布选项。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>异步任务。</returns>
        Task PublishAsync(Type type, object body, PublishOptions? options = null, CancellationToken cancellationToken = default);
        /// <summary>
        /// 订阅消息。
        /// </summary>
        /// <typeparam name="TBody">实体的数据类型。</typeparam>
        /// <param name="handler">处理方法。</param>
        /// <param name="options">订阅选项。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>一个需要手动取消的异步任务。</returns>
        Task SubscribeAsync<TBody>(AsyncEventHandler<SubscribeEventArgs<TBody>> handler, SubscribeOptions? options = null, CancellationToken cancellationToken = default);
    }

    abstract class DefaultMessageBusBase : ObjectDisposableBase, IMessageBusCore
    {
        protected readonly IChannelProxyPool _channelPool;
        protected readonly IMessageFactory _messageFactory;
        protected readonly IMessageSerializer _messageSerializer;

        public DefaultMessageBusBase(IChannelProxyPool channelPool
            , IMessageFactory messageFactory
            , IMessageSerializer messageSerializer)
        {
            this._channelPool = channelPool;
            this._messageFactory = messageFactory;
            this._messageSerializer = messageSerializer;
        }
        public IMessageManager Manager => this._channelPool.Manager;

        public abstract TTask CallAsync<TTask>(IMessage message, Func<IChannelProxy, IMessage, TTask> channelAction, string? connectionName) where TTask : Task;

        private static IBasicProperties? CreatePublishPropertie(IModel channel, PublishOptions? options)
        {
            IBasicProperties? properties = null;
            if (options is not null)
            {
                if (options.PropertiesSetup is not null)
                {
                    properties = channel.CreateBasicProperties();
                    options.PropertiesSetup(properties);
                }

                if (options.DelayTimeSpan != default)
                {
                    if (properties is null) properties = channel.CreateBasicProperties();
                    if (properties.Headers is null) properties.Headers = new Dictionary<string, object>();
                    properties.Headers.Add("x-delay", (int)options.DelayTimeSpan.TotalMilliseconds); //- 此处如果不是 int 类型会导致延迟失败
                }

            }
            return properties;
        }

        protected virtual IMessage GetMessage<TBody>()
        {
            return GetMessage(typeof(TBody));
        }

        protected virtual IMessage GetMessage(Type type)
        {
            var message = this._messageFactory.Get(type);
            if (message is null) throw new InvalidOperationException($"The type '{type.FullName}' is not declare message type.");
            return message;
        }

        public virtual Task PublishAsync<TBody>(TBody body, PublishOptions? options = null, CancellationToken cancellationToken = default)
        {
            Check.NotNull(body, nameof(body));
            return PublishAsync(typeof(TBody), body, options, cancellationToken);
        }

        public virtual Task PublishAsync(Type type, object body, PublishOptions? options = null, CancellationToken cancellationToken = default)
        {
            Check.NotNull(type, nameof(type));

            this.ThrowIfDisposed();

            return this.CallAsync<Task>(this.GetMessage(type), async (channelProxy, message) =>
            {
                var channel = channelProxy.Channel.MustBe();
                var properties = CreatePublishPropertie(channel, options);
                var routerKey = options?.RouterKey ?? string.Empty;

                Exception? exception = null;

                var bytes = this._messageSerializer.Serialize(body);
                for (int i = 0; i < this.Manager.Options.PublishRetryCount; i++)
                {
                    if (cancellationToken.IsCancellationRequested) throw new TaskCanceledException();
                    try
                    {
                        channel.BasicPublish(message.Exchange, routerKey, options?.Mandatory ?? false, properties, bytes);
                        break;
                    }
                    catch (Exception ex)
                    {
                        this.Manager.EmitError(this, ex);
                        exception = ex;
                        if (!MQueueExtensions.IsContinueOnError(ex)) break;
                        if (!channelProxy.IsOpen) break;
                        await Task.Delay(TimeSpan.FromSeconds(this.Manager.Options.PublishRetrySeconds));
                    }
                }

                if (exception is not null) throw exception;
            }, options?.ConnectionName);
        }

        public virtual Task SubscribeAsync<TBody>(AsyncEventHandler<SubscribeEventArgs<TBody>> handler, SubscribeOptions? options = null, CancellationToken cancellationToken = default)
        {
            if (handler is null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            this.ThrowIfDisposed();

            return this.CallAsync<Task>(this.GetMessage<TBody>(), async (channelProxy, message) =>
            {
                var channel = channelProxy.Channel.MustBe();

                var completionSource = new TaskCompletionSource<bool>();

                using (cancellationToken.Register(() =>
                {
                    completionSource.TrySetCanceled();
                }))
                {
                    var autoAck = options?.AutoAck ?? true;
                    var mandatory = options?.Mandatory ?? false;
                    var qosPrefetchCount = options?.QosPrefetchCount ?? 0;

                    var consumer = new EventingBasicConsumer(channel);
                    const string HEADER_REDELIVERED_COUNT = "redelivered_count";
                    consumer.Received += async (object? model, BasicDeliverEventArgs ea) =>
                    {
                        var redeliveredCount = ea.BasicProperties.Headers is not null && ea.BasicProperties.Headers.TryGetValue(HEADER_REDELIVERED_COUNT, out var obj) && obj is int count ? count : 0;
                        if (redeliveredCount > 0) ea.Redelivered = true;
                        var bodyBytes = ea.Body.ToArray();
                        var eventArgs = new SubscribeEventArgs<TBody>
                        {
                            Redelivered = ea.Redelivered,
                            RedeliveredCount = redeliveredCount
                        };
                        try
                        {
                            if (this._messageSerializer.Deserialize(typeof(TBody), bodyBytes) is not TBody body)
                                throw new InvalidCastException($"Cannot cast message body to {typeof(TBody)} type.");
                            eventArgs.Body = body;
                        }
                        catch (Exception ex)
                        {
                            this.Manager.EmitError(this, ex);
                            return;
                        }

                        try
                        {
                            await handler(this, eventArgs);
                        }
                        catch (Exception ex)
                        {
                            this.Manager.EmitError(this, ex);
                            eventArgs.Ack = false;
                            eventArgs.Requeue = true;
                        }
                        if (eventArgs.Requeue && (autoAck || !eventArgs.Ack))
                        {
                            if (ea.BasicProperties.Headers is null) ea.BasicProperties.Headers = new Dictionary<string, object>();
                            ea.BasicProperties.Headers[HEADER_REDELIVERED_COUNT] = redeliveredCount + 1;
                        }
                        if (!autoAck)
                        {
                            if (eventArgs.Ack)
                            {
                                channel.BasicAck(ea.DeliveryTag, eventArgs.Multiple);
                            }
                            else
                            {
                                if (eventArgs.Requeue && !eventArgs.Multiple)
                                {
                                    channel.BasicNack(ea.DeliveryTag, false, false);
                                    channel.BasicPublish(ea.Exchange, ea.RoutingKey, mandatory, ea.BasicProperties, bodyBytes);
                                }
                                else
                                {
                                    channel.BasicNack(ea.DeliveryTag, eventArgs.Multiple, eventArgs.Requeue);
                                }
                            }
                        }
                        else
                        {
                            if (eventArgs.Requeue && eventArgs.Ack)
                            {
                                channel.BasicPublish(ea.Exchange, ea.RoutingKey, mandatory, ea.BasicProperties, bodyBytes);
                            }
                        }
                    };
                    consumer.Shutdown += (object? ss, ShutdownEventArgs ee) =>
                    {
                        if (!completionSource.Task.IsCanceled)
                        {
                            completionSource.TrySetException(new global::RabbitMQ.Client.Exceptions.ConnectFailureException($"Message consumer is shutdown <{ee.ReplyCode}> {ee.ReplyText}", null));
                        }
                    };

                    if (qosPrefetchCount > 0)
                    {
                        channel.BasicQos(0, qosPrefetchCount, false);
                    }

                    var tag = channel.BasicConsume(queue: message.Queue, autoAck: autoAck, consumer: consumer);
                    Exception? exception = null;
                    try
                    {
                        await completionSource.Task;
                    }
                    catch (TaskCanceledException) { }
                    catch (Exception ex)
                    {
                        this.Manager.EmitError(this, ex);
                        exception = ex;
                    }
                    try
                    {
                        channel.BasicCancel(tag);
                    }
                    catch (Exception) { }
                    if (exception is not null) throw exception;
                }
            }, options?.ConnectionName);

        }

        public virtual Task BatchPublish<TBody>(IEnumerable<TBody> bodies, PublishOptions? options = null, CancellationToken cancellationToken = default)
        {
            if (bodies is null)
            {
                throw new ArgumentNullException(nameof(bodies));
            }

            this.ThrowIfDisposed();

            return this.CallAsync<Task>(this.GetMessage<TBody>(), async (channelProxy, message) =>
            {
                var channel = channelProxy.Channel.MustBe();
                var properties = CreatePublishPropertie(channel, options);
                var basicPublishBatch = channel.CreateBasicPublishBatch();
                var routerKey = options?.RouterKey ?? string.Empty;

                foreach (var body in bodies)
                {
                    basicPublishBatch.Add(message.Exchange, routerKey, options?.Mandatory ?? false, properties, new ReadOnlyMemory<byte>(this._messageSerializer.Serialize(body)));
                }
                Exception? exception = null;

                for (int i = 0; i < this.Manager.Options.PublishRetryCount; i++)
                {
                    try
                    {
                        basicPublishBatch.Publish();
                        break;
                    }
                    catch (Exception ex)
                    {
                        this.Manager.EmitError(this, ex);
                        exception = ex;
                        if (!MQueueExtensions.IsContinueOnError(ex)) break;
                        if (!channelProxy.IsOpen) break;
                        await Task.Delay(TimeSpan.FromSeconds(this.Manager.Options.PublishRetrySeconds));
                    }
                }
                if (exception is not null) throw exception;
            }, options?.ConnectionName);
        }

        protected override void DisposeManaged()
        {
        }
    }

}
