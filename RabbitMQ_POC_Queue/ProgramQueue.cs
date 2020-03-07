using RabbitMQ.Client;
using System;

namespace RabbitMQ_POC_Queue
{
    public class ProgramQueue
    {
        private static IModel channel;

        public static void Main()
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(uriString: "amqp://guest:guest@localhost"),
                ContinuationTimeout = TimeSpan.MaxValue
            };

            Console.Write(value: "Queues count: ");
            var queuesCount = int.Parse(s: Console.ReadLine());

            for (var queueIndex = 0; queueIndex < queuesCount; queueIndex++)
            {
                Console.Write(value: $"Queue[{queueIndex}] name: ");
                var queueName = Console.ReadLine();

                Console.Write(value: $"Routing key for Queue[{queueIndex}]: ");
                var routingKey = Console.ReadLine();

                using (IConnection connection = factory.CreateConnection())
                {
                    using (channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: queueName,
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

                        channel.QueueBind(queue: queueName,
                            exchange: "test-exchange",
                            routingKey: routingKey);
                    }
                }

                Console.WriteLine(value: $"Queue {queueName} created.");
            }

            Console.WriteLine(value: $"{queuesCount} queue(s) created.");

            Console.ReadKey();
        }
    }
}
