﻿using RabbitMQ.Client;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace abonent2
{
    class MyConsumer : DefaultBasicConsumer
    {
        public MyConsumer(IModel model) : base(model) { }
        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool
            redelivered, string exchange, string routingKey, IBasicProperties properties,
            ReadOnlyMemory<byte> body)
        {
            var message = Encoding.UTF8.GetString(body.ToArray());
            Console.WriteLine($"Odebrano - \"{message}\"");
        }
    }

    internal class Abonent2
    {
        static void Main(string[] args)
        {
            Thread.Sleep(500);
            Console.WriteLine("Abonent 2");
            var factory = new ConnectionFactory()
            {
                UserName = "guest",
                Password = "guest",
                HostName = "localhost",
                VirtualHost = "/"
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare("abc", "topic");
                var queueName6 = channel.QueueDeclare().QueueName;
                channel.QueueBind(queueName6, "abc", "#.xyz");
                var consumer = new MyConsumer(channel);
                channel.BasicConsume(queueName6, true, consumer);
                Task.Run(() => Console.ReadKey()).Wait();
            }
        }
    }
}
