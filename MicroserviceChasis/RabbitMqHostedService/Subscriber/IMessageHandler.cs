using System.Threading.Tasks;

namespace RabbitMqHostedService.Subscriber
{
    public interface IMessageHandler<T> where T : class
    {
        Task HandleAsync(T message);
    }
}
