using System.MQueue;
using Quartz;
using ViazyNetCore.TaskScheduler;

namespace ViazyNetCore.Manage.WebApi.Tasks
{
    [Message(Exchange = "ViazyNetCore.MqTestModel.Exchange", Queue = "ViazyNetCore.MqTestModel.Queue")]
    public class MqTestModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }

    [DisallowConcurrentExecution]
    public class Job_RabbitMqTest : RabbitMqJobBase<MqTestModel>
    {
        public Job_RabbitMqTest(IMessageBus bus, ILogger<Job_RabbitMqTest> logger, IServiceProvider serviceProvider) : base(bus, logger, serviceProvider)
        {
        }

        public override SubscribeOptions SubscribeOptions => new() { };

        protected override Task OnSubscribe(IMessageBusContext context, SubscribeEventArgs<MqTestModel> args, CancellationToken cancellationToken)
        {
            Console.Out.WriteLine("cancellationToken:{0}", cancellationToken.IsCancellationRequested);
            Console.Out.WriteLine(JSON.Stringify(args.Body));
            return Task.CompletedTask;
        }
    }
}
