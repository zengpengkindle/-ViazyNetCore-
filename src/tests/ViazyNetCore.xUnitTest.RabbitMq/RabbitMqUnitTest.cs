using System.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ViazyNetCore.Configuration;
using Xunit.Abstractions;

namespace ViazyNetCore.xUnitTest.RabbitMq
{
    public class RabbitMqUnitTest
    {

        private static IConfiguration _configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("rabbitmq.json").Build();
        [Fact]
        public async Task Test1()
        {
            //var appSettingsHelper = new AppSettingsHelper(Environment.CurrentDirectory);

            var serviceProvider = new ServiceCollection()
                .AddSingleton(sp => _configuration)
                .AddMQueue()
                .BuildServiceProvider();

            var bus = serviceProvider.GetService<IMessageBus>();
            if (bus == null)
                throw new ArgumentNullException(nameof(bus));
            using var context = bus.Context;
            var i = 0;
            do
            {
                await context.PublishAsync(new MqTestModel());
            } while (i++ < 100);
            //await context.SubscribeAsync<MqTestModel>((ss, ee) => OnSubscribe(context, ee)); ;
        }

        private static async Task OnSubscribe<TBody>(IMessageBusContext context, SubscribeEventArgs<TBody> args)
        {
            Console.WriteLine(JSON.Stringify(args.Body));
            args.Ack = true;
            //args.Requeue = false;
        }

        [Message(Exchange= "ViazyNetCore.MqTestModel.Exchange",Queue = "ViazyNetCore.MqTestModel.Queue")]
        public class MqTestModel
        {
            public Guid Id { get; set; } = Guid.NewGuid();
        }
    }
}