// See https://aka.ms/new-console-template for more information
using System.MQueue;
using System.Tests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ViazyNetCore.CommondTest;

var data = new Dictionary<string, string>()
            {
                { "MQueue:ConnectionStrings:Default", "host=127.0.0.1:5673;vhost=test;user=guest;password=guest;name=Aoite.RabbitMQ.Shell"}
                ,{ "MQueue:Options:ChannelInactiveSeconds", "15"}
            };

var configuration = new ConfigurationBuilder()
    .AddInMemoryCollection(data)
    .Build();
var services = new ServiceCollection()
    .AddSingleton<IConfiguration>(configuration)
    .AddMQueue()
    .AddLogging(builder => builder.AddConsole())
    .BuildServiceProvider();

var loggerFactory = services.GetRequiredService<ILoggerFactory>();
var bus = services.GetRequiredService<IMessageBus>();

loggerFactory.CreateLogger<Program>().LogInformation("启动...");
bus.Manager.Error += (ss, ee) =>
{
    loggerFactory.CreateLogger(ss.GetType().FullName).LogError(ee.Exception, "发生了严重错误");
};


var cancellationTokenSource = new CancellationTokenSource();

CommandMethodFactory.Create("MessageBus")
    .Add("发布", m =>
    {
        var delayS = m.Get("延迟秒数", 5);
        var count = m.Get("发送次数", 5);
        var auto = m.Get("自动测试", false);
        if (auto)
        {
            var cancellationToken = new CancellationTokenSource();

            for (int y = 0; y < count; y++)
            {
                Task.Factory.StartNew(async o =>
                {
                    var lastActiveTime = DateTime.MinValue;
                    m.WriteInfo($"{o} 立即开始");
                    try
                    {
                        while (!cancellationToken.IsCancellationRequested)
                        {
                            using (var context = bus.Context)
                            {
                                for (int i = 0; i < FastRandom.Instance.Next(count - 2, count + 2); i++)
                                {
                                    await context.PublishAsync(new NotifyBody
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        Amount = FastRandom.Instance.Next(500, 1000),
                                        ModifyTime = DateTime.Now,
                                        Status = 1,
                                    }, FastRandom.Instance.Next(delayS - 2, delayS + 2));
                                }
                            }
                            await Task.Delay(FastRandom.Instance.Next(5000, 10000));
                            if ((DateTime.Now - lastActiveTime).TotalSeconds > 20)
                            {
                                var f = services.GetRequiredService<IMessageManager>().Default.MustBe();
                                var cs = f.Connections.ToArray();
                                for (int i = 0; i < cs.Length; i++)
                                {
                                    Console.Title = ($"#{i} channel count {cs[i].ChannelCount}   usage count : {cs[i].UsageChannelCount}  busy count : {cs[i].BusyCount}  unbusy count : {cs[i].FreeCount}");
                                }
                                lastActiveTime = DateTime.Now;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        //System.IO.StringWriter
                        m.WriteError(e.ToString());
                    }
                    m.WriteInfo($"{o} 已结束");
                }, y);
            }
            while (!m.Get("取消", false))
            {

            }
            cancellationToken.Cancel();
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                Task.Factory.StartNew(async o =>
                {
                    m.WriteInfo($"{o} 开始发布");
                    await Task.Delay(FastRandom.Instance.Next(0, 100));
                    await bus.PublishAsync(new NotifyBody
                    {
                        Id = Guid.NewGuid().ToString(),
                        Amount = FastRandom.Instance.Next(500, 1000),
                        ModifyTime = DateTime.Now,
                        Status = 1,
                    }, delayS);

                    m.WriteInfo($"{o} 发布成功");
                }, i);
            }
        }
    })
    .Add("订阅", m =>
    {
        cancellationTokenSource = new CancellationTokenSource();
        var count = m.Get("订阅线程", 1);
        for (int i = 0; i < count; i++)
        {
            Task.Factory.StartNew(async o =>
            {
                using var context = bus.Context;
                m.WriteInfo($"{o} 已启动...");
                while (true)
                {
                    try
                    {
                        await context.SubscribeAsync<NotifyBody>((ss, ee) =>
                        {
                            m.WriteInfo(JSON.Stringify(ee.Body));
                            ee.Ack = true;
                            return Task.CompletedTask;
                        }, new() { QosPrefetchCount = 1, AutoAck = false }, cancellationTokenSource.Token);
                        break; //- 正常取消，中断订阅
                    }
                    catch (Exception ex)
                    {
                        //- 抛出错误则表示非手动取消
                        m.WriteError(ex.ToString());
                    }
                }

                m.WriteInfo($"{o} 已停止...");
            }, i);
        }
    })
    .Add("取消订阅", m =>
    {
        cancellationTokenSource.CancelAfter(1000);
    })
    .Add("信息", m =>
    {
        var f = services.GetRequiredService<IMessageManager>().Default.MustBe();
        var cs = f.Connections.ToArray();
        m.WriteInfo($"Total connections count : {cs.Length}");
        for (int i = 0; i < cs.Length; i++)
        {
            m.WriteInfo($"#{i} channel count {cs[i].ChannelCount} \tusage count : {cs[i].UsageChannelCount}\tbusy count : {cs[i].BusyCount}\tunbusy count : {cs[i].FreeCount}");
        }
        var items = f.Connections.SelectMany(c => c.Channels.Where(c => c.IsBusy).Select(c => new { c.Id, c.Reason, c.LastActiveTime })).ToArray();
        m.WriteTable(items);
    })
    .Run();
