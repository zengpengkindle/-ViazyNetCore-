﻿using System.MQueue;
using Quartz;
using ViazyNetCore.TaskScheduler;

namespace ViazyNetCore.Manage.WebApi.Tasks
{
    [EventName("ViazyNetCore.MqTestModel.Exchange")]
    [Message(Exchange = "ViazyNetCore.MqTestModel.Exchange", Queue = "ViazyNetCore.MqTestModel.Queue")]
    public class MqTestModel : IEventData
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime EventTime { get; set; }
    }

    [DisallowConcurrentExecution]
    public class Job_RabbitMqTest : RabbitMqJobBase<MqTestModel>, IDistributedEventHandler<MqTestModel>
    {
        public Job_RabbitMqTest(IMessageBus bus, ILogger<Job_RabbitMqTest> logger, IServiceProvider serviceProvider)
            : base(bus, logger, serviceProvider)
        {
        }

        public override SubscribeOptions SubscribeOptions => new() { };

        public async Task HandleEventAsync(MqTestModel eventData)
        {
            using var context = this.Bus.Context;
            await context.SubscribeAsync<MqTestModel>((ss, ee) => this.OnSubscribe(context, ee, CancellationToken.None)
            , this.SubscribeOptions
            , cancellationToken: CancellationToken.None);
        }

        protected override Task OnSubscribe(IMessageBusContext context, SubscribeEventArgs<MqTestModel> args, CancellationToken cancellationToken)
        {
            Console.Out.WriteLine("cancellationToken:{0}", cancellationToken.IsCancellationRequested);
            Console.Out.WriteLine(JSON.Stringify(args.Body));
            return Task.CompletedTask;
        }
    }
}
