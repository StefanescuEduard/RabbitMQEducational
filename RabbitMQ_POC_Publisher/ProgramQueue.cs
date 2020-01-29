using RabbitMQ.Client;
using System;

namespace RabbitMQ_POC_Publisher
{
	public class ProgramQueue
	{
		private static IModel channel;

		public static void Main()
		{
			Console.Write("Queues count: ");
			var queueCount = int.Parse(Console.ReadLine());

			for (var queueIndex = 0; queueIndex < queueCount; queueIndex++)
			{
				Console.Write($"Queue[{queueIndex}] name: ");
				var queueName = Console.ReadLine();

				Console.Write($"Routing key for Queue[{queueIndex}]: ");
				var routingKey = Console.ReadLine();

				var factory = new ConnectionFactory
				{
					Uri = new Uri("amqp://guest:guest@localhost"),
					ContinuationTimeout = TimeSpan.MaxValue
				};

				using (IConnection connection = factory.CreateConnection())
				{
					using (channel = connection.CreateModel())
					{
						channel.QueueDelete(queueName);
						channel.QueueDeclare(queueName, false, false, false, null);

						channel.QueueBind(queueName, "test-exchange", routingKey);
					}
				}

				Console.WriteLine($"Queue {queueName} created.");
			}

			Console.ReadKey();
		}
	}
}
