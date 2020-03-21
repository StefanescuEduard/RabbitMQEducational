using RabbitMQ.Client;
using System;

namespace RabbitMQ_POC_HeadersExchange
{
    public class ProgramHeadersExchange
    {
        public static void Main()
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost"),
                ContinuationTimeout = TimeSpan.MaxValue
            };

            using (IConnection connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    const string exchangeName = "test-exchange";
                    channel.ExchangeDelete(exchange: exchangeName);
                    channel.ExchangeDeclare(exchange: exchangeName, type: "headers");
                }
            }

            Console.WriteLine("Headers Exchange named test-exchange was created successfully.");
            Console.ReadKey();
        }
    }
}
