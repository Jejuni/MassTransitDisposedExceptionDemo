using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using GreenPipes;
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
                    rabbitMqConfig.UseDelayedExchangeMessageScheduler();
                    rabbitMqConfig.UseInMemoryOutbox();
                    rabbitMqConfig.PrefetchCount = 16;
                    rabbitMqConfig.UseMessageRetry(x => x.Exponential(4, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(4), TimeSpan.FromSeconds(1)));

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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
