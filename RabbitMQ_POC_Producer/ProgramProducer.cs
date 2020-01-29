using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMQ_POC_Producer
{
	public class ProgramProducer
	{
		public static void Main()
		{
			while (true)
			{

				Console.Write("Routing key: ");
				var routingKey = Console.ReadLine();

				Console.Write("Message: ");
				var message = Console.ReadLine();

				var factory = new ConnectionFactory
				{
					Uri = new Uri("amqp://guest:guest@localhost"),
					ContinuationTimeout = TimeSpan.MaxValue
				};

				using (IConnection connection = factory.CreateConnection())
				{
					using (IModel channel = connection.CreateModel())
					{
						var properties = channel.CreateBasicProperties();
						properties.Persistent = false;
						var messageBytes = Encoding.UTF8.GetBytes(message);

						channel.BasicPublish("test-exchange", routingKey, properties, messageBytes);
						Console.WriteLine($"Published message: {message}");
					}
				}
			}
		}
	}
}
