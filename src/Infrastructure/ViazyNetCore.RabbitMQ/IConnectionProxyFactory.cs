using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using RabbitMQ.Client;

namespace System.MQueue
{
    /// <summary>
    /// 定义一个连接代理工厂。
    /// </summary>
    public interface IConnectionProxyFactory
    {
        /// <summary>
        /// 获取连接的名称。
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 获取消息管理器。
        /// </summary>
        IMessageManager Manager { get; }
        /// <summary>
        /// 获取连接工厂。
        /// </summary>
        ConnectionFactory Factory { get; }
        /// <summary>
        /// 获取已经创建的连接数量。
        /// </summary>
        long CreatedCount { get; }
        /// <summary>
        /// 获取所有的连接代理。
        /// </summary>
        IEnumerable<IConnectionProxy> Connections { get; }
        /// <summary>
        /// 创建一个基于客户端提供程序名称的连接代理。
        /// </summary>
        /// <returns>连接代理。</returns>
        IConnectionProxy Create();
    }


    class DefaultConnectionProxyFactory : IConnectionProxyFactory
    {
        private readonly AmqpTcpEndpoint[] _endpoints;
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, Lazy<IConnectionProxy>>> _connections;
        private long _createdCount;

        public string Name { get; }

        public IDictionary<string, string> Configuration { get; }

        public ConnectionFactory Factory { get; }

        public IEnumerable<IConnectionProxy> Connections => this._connections.Values.SelectMany(c => c.Values.Select(v => v.Value));

        public long CreatedCount => Interlocked.Read(ref this._createdCount);

        public IMessageManager Manager { get; }

        public DefaultConnectionProxyFactory(IMessageManager messageManager, string name, IDictionary<string, string> configuration)
        {
            this._connections = new ConcurrentDictionary<string, ConcurrentDictionary<string, Lazy<IConnectionProxy>>>(StringComparer.OrdinalIgnoreCase);

            this.Manager = messageManager;
            this.Name = name;
            this.Configuration = configuration;

            var hosts = this.GetString("hosts", "host", "hostname")?.Split(',');
            if(hosts is null) throw new InvalidCastException("Cannot found the RabbitMQ hosts.");
            this._endpoints = new AmqpTcpEndpoint[hosts.Length];
            for(int i = 0; i < hosts.Length; i++)
            {
                var port = 5672;
                var segs = hosts[i].Split(':');
                var hostname = segs[0];
                if(segs.Length == 2)
                {
                    port = segs[1].ParseTo<int>();
                }
                else if(segs.Length > 2)
                {
                    throw new FormatException($"Parse the host string '{hosts[i]}' error.");
                }

                this._endpoints[i] = new AmqpTcpEndpoint(hostname, port);
            }
            this.Factory = this.CreateFactory();

            if(messageManager.Options.PreheatCount > 0)
            {
                var proxy = this.Create();
                while(proxy.ChannelCount < messageManager.Options.PreheatCount)
                {

                    proxy.GetChannel(true).MustBe().TryFree();
                }
            }

        }

        public static IDictionary<string, TValue> ParserConnectionString<TValue>(string? connectionString)
        {
            if(connectionString is null) return new Dictionary<string, TValue>(0);
            try
            {
                var builder = new System.Data.Common.DbConnectionStringBuilder() { ConnectionString = connectionString };
                var dict = new Dictionary<string, TValue>(builder.Count, StringComparer.OrdinalIgnoreCase);
                foreach(string item in builder.Keys)
                {
                    dict.Add(item, (TValue)builder[item]);
                }
                return dict;
            }
            catch(Exception ex)
            {
                throw new FormatException($"The connection string '{connectionString}' is fail format.", ex);
            }
        }

        private ConnectionFactory CreateFactory()
        {
            return new ConnectionFactory
            {
                AutomaticRecoveryEnabled = true,
                TopologyRecoveryEnabled = false,
                //DispatchConsumersAsync = true,
                VirtualHost = this.GetString("VirtualHost", "vhost") ?? ConnectionFactory.DefaultVHost,
                UserName = this.GetString("UserName", "user") ?? ConnectionFactory.DefaultUser,
                Password = this.GetString("Password", "pwd", "pass") ?? ConnectionFactory.DefaultPass,
                RequestedHeartbeat = this.GetTimeSpan(ConnectionFactory.DefaultHeartbeat, "RequestedHeartbeat", "heartbeat"),
                NetworkRecoveryInterval = this.GetTimeSpan(TimeSpan.FromSeconds(5), "NetworkRecoveryInterval", "interval"),
                ContinuationTimeout = this.GetTimeSpan(ConnectionFactory.DefaultConnectionTimeout, "ContinuationTimeout", "timeout"),
                ClientProvidedName = this.GetString("Name", "provider", "client") ?? Assembly.GetExecutingAssembly().GetName().FullName,
                ClientProperties = ParserConnectionString<object>(this.GetString("ClientProperties", "properties")) ?? new Dictionary<string, object>(0),
                RequestedChannelMax = this.GetString("MaxChannels").ParseTo<ushort?>() ?? ConnectionFactory.DefaultChannelMax,
            };
        }

        private Lazy<IConnectionProxy> CreateProxy(string clientProviderName)
        {
            return new Lazy<IConnectionProxy>(() =>
            {
                Exception? exception = null;
                for(int i = 0; i < this.Manager.Options.ConnectionCreateRetryCount; i++)
                {
                    try
                    {
                        var proxy = new DefaultConnectionProxy(clientProviderName, this.Factory.CreateConnection(this._endpoints, clientProviderName), this);
                        Interlocked.Increment(ref this._createdCount);
                        return proxy;
                    }
                    catch(Exception ex)
                    {
                        this.Manager.EmitError(this, ex);
                        exception = ex;
                        if(!MQueueExtensions.IsContinueOnError(ex))
                        {
                            break;
                        }
                        GA.Wait(this.Manager.Options.ConnectionCreateRetrySeconds);
                    }
                }
                throw exception!;
            });
        }

        private ConcurrentDictionary<string, Lazy<IConnectionProxy>> CreateProxies(string _)
        {
            return new ConcurrentDictionary<string, Lazy<IConnectionProxy>>(StringComparer.OrdinalIgnoreCase);
        }

        public IConnectionProxy Create()
        {
            var clientProviderName = this.Factory.ClientProvidedName;

            IConnectionProxy data;
            //TRY_AGENT:
            lock(string.Intern(clientProviderName))
            {
                var dict = this._connections.GetOrAdd(clientProviderName, this.CreateProxies);
                var key = clientProviderName;
                try
                {
                    Lazy<IConnectionProxy> lazy;
                    if(dict.IsEmpty)
                    {
                        key = dict.Count + "#" + clientProviderName;
                        lazy = dict.GetOrAdd(key, this.CreateProxy);
                    }
                    else
                    {
                        var item = dict.OrderBy(c => c.Value.Value.UsageChannelCount).First();
                        key = item.Key;
                        lazy = item.Value;
                    }

                    data = lazy.Value;
                }
                catch(Exception)
                {
                    if(dict.TryRemove(key, out var c)) c.Value?.Dispose();
                    throw;
                }
                var conn = data.Connection;
                if(conn?.IsOpen == true)
                {
                    var mpc = this.Manager.Options.MaxChannelPerConnection;
                    var usageChannelCount = data.UsageChannelCount;
                    if(usageChannelCount > mpc || (dict.Count < 5 && usageChannelCount > (mpc / 2)))
                    {
                        var newKey = (dict.Count + 1) + "#" + clientProviderName;
                        Task.Factory.StartNew(this.TryCreateChannel, Tuple.Create(dict, newKey));
                    }
                    return data;
                }
                else
                {
                    data?.Dispose();
                }
            }

            this._connections.TryRemove(clientProviderName, out _);
            return this.Create();
        }

        private void TryCreateChannel(object? state)
        {
            if(state is Tuple<ConcurrentDictionary<string, Lazy<IConnectionProxy>>, string> item)
            {
                var dict = item.Item1;
                var newKey = item.Item2;

                if(dict.ContainsKey(newKey)) return;
                lock(dict)
                {
                    if(dict.ContainsKey(newKey)) return;
                    var newLazy = this.CreateProxy(newKey);
                    if(newLazy.Value.IsOpen)
                        dict.TryAdd(newKey, newLazy);
                }
            }
        }

        private string? GetString(params string[] keys)
        {
            string? value = null;
            foreach(var key in keys)
            {
                if(this.Configuration.TryGetValue(key, out value)) break;
            }
            return value;
        }

        private TimeSpan GetTimeSpan(TimeSpan defaultValue, params string[] keys)
        {
            var value = this.GetString(keys).ParseTo<int?>();
            if(value is null) return defaultValue;
            return TimeSpan.FromSeconds(value.Value);
        }
    }
}
