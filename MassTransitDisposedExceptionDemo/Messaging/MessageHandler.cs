using System;
using System.Threading.Tasks;
using MassTransit;

namespace MassTransitDisposedExceptionDemo.Messaging
{
    public class MyFirstMessage { }

    public class MySecondMessage { }


    public class MyFirstMessageHandler: IConsumer<MyFirstMessage>
    {
        public Task Consume(ConsumeContext<MyFirstMessage> context) => context.Send(new MySecondMessage());
    }

    public class MySecondMessageHandler : IConsumer<MySecondMessage>
    {
        public Task Consume(ConsumeContext<MySecondMessage> context)
        {
            Console.WriteLine("Works!");
            return Task.CompletedTask;
        }
    }
}
