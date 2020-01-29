using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMQ_POC_Consumer
{
	public class ProgramConsumer
	{
		private static IModel channel;
		private static EventingBasicConsumer consumer;

		public static void Main()
		{
			Console.Write("Listening queues to this consumer: ");
			int listeningQueuesCount = int.Parse(Console.ReadLine());

			var factory = new ConnectionFactory
			{
				Uri = new Uri("amqp://guest:guest@localhost"),
				ContinuationTimeout = TimeSpan.MaxValue
			};
			IConnection connection = factory.CreateConnection();
			channel = connection.CreateModel();
			consumer = new EventingBasicConsumer(channel);

			for (int queueIndex = 0; queueIndex < listeningQueuesCount; queueIndex++)
			{
				Console.Write($"Queue[{queueIndex}] name: ");
				var queueName = Console.ReadLine();

				consumer.Received += ConsumerOnReceived;
				channel.BasicConsume(queueName, false, consumer);
			}

			Console.WriteLine(
				$"Consumer created successfully and listening for {listeningQueuesCount} queue(s).");
			Console.ReadKey();
		}

		private static void ConsumerOnReceived(object sender, BasicDeliverEventArgs e)
		{
			Console.WriteLine($"Message for {e.RoutingKey}: {Encoding.UTF8.GetString(e.Body)}");

			channel.BasicAck(e.DeliveryTag, true);
		}
	}
}