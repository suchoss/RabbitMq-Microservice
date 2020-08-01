using Messages;
using RabbitMqHostedService.Subscriber;
using System;
using System.Threading.Tasks;

namespace Microservice
{
    public class ProcessSimpleMessage : IMessageHandler<SimpleMessage>
    {
        // this part simulates processing of message
        public async Task HandleAsync(SimpleMessage message)
        {
            Console.WriteLine($"{message.SomeNumber} started processing - {message.SomeValue}");
            await Task.Delay(2000);
            Console.WriteLine($"{message.SomeNumber} finished processing.");
        }
    }
}