using System.Threading.Tasks;
using GreenPipes;
using MassTransit;

namespace MassTransitDisposedExceptionDemo.Messaging
{
    public class MySendFilter<TCommand> : IFilter<SendContext<TCommand>> where TCommand : class
    {
        public Task Send(SendContext<TCommand> context, IPipe<SendContext<TCommand>> next) => next.Send(context);

        public void Probe(ProbeContext context) { }
    }
}
