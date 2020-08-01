using EasyNetQ;
using Messages;

namespace Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "host=localhost;username=guest;password=guest";

            for(int i = 0; i < 100; i++ )
            {
                using var bus = RabbitHutch.CreateBus(connectionString);
                var message = new SimpleMessage()
                {
                    SomeNumber = i,
                    SomeValue = "This is a message sent to RabbitMq"
                };

                bus.Publish<SimpleMessage>(message);
            }
        }
    }
}
