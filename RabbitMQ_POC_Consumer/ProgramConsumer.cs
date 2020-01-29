using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

namespace RabbitMQ_POC_Consumer
{
	public class ProgramConsumer
	{
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
			IConnection connection = factory.CreateConnection();
			channel = connection.CreateModel();

			var cancellationTokenSource = new CancellationTokenSource();

			for (var queueIndex = 0; queueIndex < listeningQueuesCount; queueIndex++)
			{
				Console.Write($"Queue[{queueIndex}] name: ");
				var queueName = Console.ReadLine();

				CancellationToken cancellationToken = cancellationTokenSource.Token;

				var thread = new Thread(() => DisplayQueueMessage(queueName, cancellationToken));
				thread.Start();
			}

			Console.WriteLine(
				$"Consumer created successfully and listening for {listeningQueuesCount} queue(s).");
			Console.ReadKey();

			cancellationTokenSource.Cancel();
		}

		private static void DisplayQueueMessage(string queueName, CancellationToken cancellationToken)
		{
			while (true)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					break;
				}

				BasicGetResult result = channel.BasicGet(queueName, false);
				if (result != null)
				{
					Console.WriteLine($"Message: {Encoding.ASCII.GetString(result.Body)}");
					channel.BasicAck(result.DeliveryTag, false);
				}
			}
		}
	}
}