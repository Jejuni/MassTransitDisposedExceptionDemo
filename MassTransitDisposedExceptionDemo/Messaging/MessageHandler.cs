using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;

namespace MassTransitDisposedExceptionDemo.Messaging
{
    public abstract class AsyncCommand : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }

        protected AsyncCommand(Guid correlationId)
        {
            if (correlationId.Equals(Guid.Empty))
                throw new ArgumentException($"{nameof(correlationId)} may not be Guid.Empty", nameof(correlationId));
            CorrelationId = correlationId;
        }

        protected AsyncCommand() : this(Guid.NewGuid()) { }
    }
    public class MyFirstMessage: AsyncCommand
    {
        public MyFirstMessage(DateTimeOffset pointInTime) => PointInTime = pointInTime;

        public DateTimeOffset PointInTime { get; init; }
    }

    public class MySecondMessage : AsyncCommand
    {
        public MySecondMessage(Guid correlationId) : base(correlationId) { }
    }


    public class MyFirstMessageHandler: IConsumer<MyFirstMessage>
    {
        public async Task Consume(ConsumeContext<MyFirstMessage> context)
        {
            await context.Send(new MySecondMessage(context.CorrelationId!.Value)); ;
        }
    }

    public class MySecondMessageHandler : IConsumer<MySecondMessage>
    {
        public Task Consume(ConsumeContext<MySecondMessage> context)
        {
            Console.WriteLine($"Got message, {context.Message.CorrelationId}!");
            return Task.CompletedTask;
        }
    }
}
