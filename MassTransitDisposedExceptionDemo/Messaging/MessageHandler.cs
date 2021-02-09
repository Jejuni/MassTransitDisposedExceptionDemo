using System;
using System.Threading.Tasks;
using MassTransit;

namespace MassTransitDisposedExceptionDemo.Messaging
{
    public class MyFirstMessage { }
    
    public class MyFirstMessageHandler: IConsumer<MyFirstMessage>
    {
        public Task Consume(ConsumeContext<MyFirstMessage> context) => throw new InvalidOperationException();
    }
    
    public class MyFaulHandler : IConsumer<Fault<MyFirstMessage>>
    {
        public Task Consume(ConsumeContext<Fault<MyFirstMessage>> context)
        {
            Console.WriteLine("Got Fault!");
            return Task.CompletedTask;
        }
    }
}
