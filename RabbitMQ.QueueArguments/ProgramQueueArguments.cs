using RabbitMQ.Client;
using System;
using System.Collections.Generic;

namespace RabbitMQ.QueueArguments
{
    public class ProgramQueueArguments
    {
        public static void Main()
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost"),
                ContinuationTimeout = TimeSpan.MaxValue
            };

            Console.Write("Queues count: ");
            var queuesCount = int.Parse(Console.ReadLine());

            for (var queueIndex = 0; queueIndex < queuesCount; queueIndex++)
            {
                Console.Write($"Queue[{queueIndex}] name: ");
                var queueName = Console.ReadLine();

                Console.Write($"Log level for Queue[{queueIndex}]: ");
                var logLevel = Console.ReadLine();

                using (IConnection connection = factory.CreateConnection())
                {
                    using (IModel channel = connection.CreateModel())
                    {
                        channel.QueueDelete(queue: queueName);

                        channel.QueueDeclare(queue: queueName,
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

                        channel.QueueBind(queue: queueName,
                            exchange: "test-exchange",
                            routingKey: string.Empty,
                            arguments: new Dictionary<string, object>
                                {{"x-match", "all"}, {"log-level", logLevel}});
                    }
                }

                Console.WriteLine(value: $"Queue {queueName} created.");
            }

            Console.WriteLine(value: $"{queuesCount} queue(s) created.");

            Console.ReadKey();
        }
    }
}
