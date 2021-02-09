using GreenPipes;
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
                    rabbitMqConfig.UseMessageRetry(x => x.Immediate(2));
                    rabbitMqConfig.UseMessageScope(busRegistrationContext);
                    rabbitMqConfig.UseInMemoryOutbox();
                    
                    rabbitMqConfig.ReceiveEndpoint("testapi", o =>
                    {
                        //o.UseMessageRetry(x => x.Immediate(2));
                        //o.UseMessageScope(busRegistrationContext);
                        //o.UseInMemoryOutbox();
                        
                        o.ConfigureConsumer<MyFirstMessageHandler>(busRegistrationContext);
                        o.ConfigureConsumer<MyFaulHandler>(busRegistrationContext);
                        EndpointConvention.Map<MyFirstMessage>(o.InputAddress);
                    });
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
