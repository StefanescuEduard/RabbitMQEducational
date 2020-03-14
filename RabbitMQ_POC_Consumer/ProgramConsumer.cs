using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

namespace RabbitMQ_POC_Consumer
{
    public class ProgramConsumer
    {
        private static IConnection connection;
        private static IModel channel;

        public static void Main()
        {
            Console.Write("Listening queues to this consumer: ");
            var listeningQueuesCount = int.Parse(Console.ReadLine());

            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost"),
                ContinuationTimeout = TimeSpan.MaxValue
            };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            var cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            for (var queueIndex = 0; queueIndex < listeningQueuesCount; queueIndex++)
            {
                Console.Write($"Queue[{queueIndex}] name: ");
                var queueName = Console.ReadLine();

                var thread = new Thread(() => DisplayQueueMessage(queueName, cancellationToken));
                thread.Start();
            }

            Console.WriteLine(
                $"Consumer created successfully and listening to {listeningQueuesCount} queue(s).");
            Console.ReadKey();

            cancellationTokenSource.Cancel();

            WaitHandle.WaitAny(new[] { cancellationToken.WaitHandle });

            channel.Close();
            channel.Dispose();
            connection.Close();
            connection.Dispose();
        }

        private static void DisplayQueueMessage(string queueName, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                BasicGetResult result = channel.BasicGet(queueName, false);
                if (result != null)
                {
                    Console.WriteLine($"Message: {Encoding.UTF8.GetString(result.Body)}");
                    channel.BasicAck(result.DeliveryTag, false);
                    Console.WriteLine("Press any key to stop consuming messages.");
                }
            }
        }
    }
}