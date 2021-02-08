using System.Threading.Tasks;
using GreenPipes;
using MassTransit;

namespace MassTransitDisposedExceptionDemo.Messaging
{
    public class MySendFilter<TCommand> :
        IFilter<SendContext<TCommand>>
        where TCommand : class
    {

        public async Task Send(SendContext<TCommand> context, IPipe<SendContext<TCommand>> next)
        {
            await next.Send(context).ConfigureAwait(false);
        }

        public void Probe(ProbeContext context)
        {
        }
    }
}
