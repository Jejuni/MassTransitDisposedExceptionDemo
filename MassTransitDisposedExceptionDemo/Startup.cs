using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using MassTransitDisposedExceptionDemo.Messaging;

namespace MassTransitDisposedExceptionDemo
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMassTransit(configurator =>
            {
                configurator.AddConsumers(typeof(Program).Assembly);

                configurator.UsingRabbitMq((busRegistrationContext, rabbitMqConfig) =>
                {
                    rabbitMqConfig.UseInMemoryOutbox();

                    rabbitMqConfig.ReceiveEndpoint("testapi", o =>
                    {
                        o.ConfigureConsumer<MySecondMessageHandler>(busRegistrationContext);
                        o.ConfigureConsumer<MyFirstMessageHandler>(busRegistrationContext);
                        EndpointConvention.Map<MySecondMessage>(o.InputAddress);
                        EndpointConvention.Map<MyFirstMessage>(o.InputAddress);
                    });

                    rabbitMqConfig.UseSendFilter(typeof(MySendFilter<>), busRegistrationContext);
                });
            });

            services.AddMassTransitHostedService();
        }
        
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
