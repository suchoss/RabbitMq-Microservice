using System.Reflection;

namespace RabbitMqHostedService.Subscriber
{
    public class SubscriberOptions
    {
        public string SubscriberName { get; set; } = Assembly.GetExecutingAssembly().GetName().Name;
        public ushort PrefetchCount { get; set; } = 5;
    }
}
