using RabbitMQ.Client;
using System;

namespace RabbitMQ_POC_Exchange
{
	public class ProgramExchange
	{
		public static void Main()
		{
			Console.Write("Exchange type (fanout, direct, topic): ");
			var exchangeType = Console.ReadLine();

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
					channel.ExchangeDelete(exchangeName);
					channel.ExchangeDeclare(exchangeName, exchangeType);
				}
			}

			Console.WriteLine(
				$"Exchange test-exchange of type {exchangeType} created successfully.");
			Console.ReadKey();
		}
	}
}