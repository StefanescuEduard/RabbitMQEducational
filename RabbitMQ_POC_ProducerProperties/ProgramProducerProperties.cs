using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ_POC_ProducerProperties
{
    public class ProgramProducerProperties
    {
        public static void Main()
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost"),
                ContinuationTimeout = TimeSpan.MaxValue
            };

            while (true)
            {
                Console.Write("Log level: ");
                var logLevel = Console.ReadLine();

                Console.Write("Message: ");
                var message = Console.ReadLine();

                var propertiesHeaders = new Dictionary<string, object> { { "log-level", logLevel } };

                using (IConnection connection = factory.CreateConnection())
                {
                    using (IModel channel = connection.CreateModel())
                    {
                        var properties = channel.CreateBasicProperties();
                        properties.Headers = propertiesHeaders;

                        var messageBytes = Encoding.UTF8.GetBytes(message);
                        const string exchangeName = "test-exchange";

                        channel.BasicPublish(exchangeName, string.Empty, properties, messageBytes);
                        Console.WriteLine($"Published message: {message}");
                    }
                }
            }
        }
    }
}