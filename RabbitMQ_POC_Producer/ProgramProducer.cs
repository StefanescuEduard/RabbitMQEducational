using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMQ_POC_Producer
{
    public class ProgramProducer
    {
        public static void Main()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
                ContinuationTimeout = TimeSpan.MaxValue
            };

            while (true)
            {
                Console.Write("Routing key: ");
                var routingKey = Console.ReadLine();

                Console.Write("Message: ");
                var message = Console.ReadLine();

                using (IConnection connection = factory.CreateConnection())
                {
                    using (IModel channel = connection.CreateModel())
                    {
                        var properties = channel.CreateBasicProperties();
                        var messageBytes = Encoding.UTF8.GetBytes(message);
                        const string exchangeName = "test-exchange";

                        channel.BasicPublish(exchangeName, routingKey, properties, messageBytes);
                        Console.WriteLine($"Published message: {message}");
                    }
                }
            }
        }
    }
}
