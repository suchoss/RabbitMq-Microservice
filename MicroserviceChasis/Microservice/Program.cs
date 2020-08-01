using EasyNetQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMqHostedService.Subscriber;
using Messages;

namespace Microservice
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                // connection to RabbitMQ bus
                services.AddSingleton<IBus>(serviceProvider =>
                {
                    string connectionString = hostContext.Configuration.GetValue<string>("RabbitMqConnectionString");
                    return RabbitHutch.CreateBus(connectionString);
                });

                // rabbit configuration
                services.Configure<SubscriberOptions>(hostContext.Configuration.GetSection("RabbitMqSubscriber"));
                services.AddHostedService<SubscriberService<SimpleMessage>>();
                services.AddScoped<IMessageHandler<SimpleMessage>, ProcessSimpleMessage>();

                // other services might be registered below
                // …
            });
    }
}
