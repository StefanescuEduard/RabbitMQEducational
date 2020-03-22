using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace RabbitMQ.ConsumerEvents
{
    public class ProgramConsumerEvents
    {
        public static void Main()
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost"),
                ContinuationTimeout = TimeSpan.MaxValue
            };

            Console.Write("Listening queues to this consumer: ");
            var listeningQueuesCount = int.Parse(Console.ReadLine());

            var cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            IConnection connection = factory.CreateConnection();
            IModel channel = connection.CreateModel();
            var basicConsumer = new EventingBasicConsumer(channel);

            basicConsumer.Received += OnNewMessageReceived;

            for (int queueIndex = 0; queueIndex < listeningQueuesCount; queueIndex++)
            {
                Console.Write($"Queue[{queueIndex}] name: ");
                var queueName = Console.ReadLine();

                channel.BasicConsume(
                    queue: queueName,
                    autoAck: true,
                    consumer: basicConsumer);
            }

            Console.WriteLine(
                $"Consumer created successfully and listening to {listeningQueuesCount} queue(s).");
            Console.ReadKey();

            cancellationTokenSource.Cancel();

            WaitHandle.WaitAny(new[] { cancellationToken.WaitHandle });

            basicConsumer.Received -= OnNewMessageReceived;
            channel.Close();
            channel.Dispose();
            connection.Close();
            connection.Dispose();
        }

        private static void OnNewMessageReceived(object sender, BasicDeliverEventArgs e)
        {
            Console.WriteLine($"Message: {Encoding.UTF8.GetString(e.Body)}");
            Console.WriteLine("Press any key to stop consuming message.");
        }
    }
}