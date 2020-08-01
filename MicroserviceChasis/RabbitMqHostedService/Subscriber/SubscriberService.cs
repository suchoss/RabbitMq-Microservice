using EasyNetQ;
using System.Threading;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace RabbitMqHostedService.Subscriber
{
    public class SubscriberService<T> : IHostedService where T : class
    {
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly ILogger _logger;
        private readonly IMessageHandler<T> _messageHandler;
        private readonly SubscriberOptions _options;
        private readonly IBus _rabbitBus;

        public SubscriberService(
            IHostApplicationLifetime appLifetime,
            IBus rabbitBus,
            ILogger<SubscriberService<T>> logger,
            IMessageHandler<T> messageHandler,
            IOptionsMonitor<SubscriberOptions> options)
        {
            _rabbitBus = rabbitBus ?? throw new ArgumentNullException(nameof(rabbitBus));
            _messageHandler = messageHandler ?? throw new ArgumentNullException(nameof(messageHandler));
            _logger = logger;
            _appLifetime = appLifetime;
            _options = options.CurrentValue;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting listener service.");
            _appLifetime.ApplicationStarted.Register(SubscribeToQueue);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Ending listener service.");
            return Task.CompletedTask;
        }

        private void SubscribeToQueue()
        {
            _logger.LogInformation("Subscribing to Queue.");
            _logger.LogInformation(_options.ToString());
            // Subscriber name should be unique for every microservice type, so different microservices
            // don't cannibalize the same queue.
            // More instances of the same microservice can access the same queue (they should have same name).
            // 
            // Prefetch count says how many messages should be pre-loaded
            _rabbitBus.SubscribeAsync<T>(_options.SubscriberName, _messageHandler.HandleAsync, configure => {
                configure.WithPrefetchCount(_options.PrefetchCount);
            });
        }
    }
}



