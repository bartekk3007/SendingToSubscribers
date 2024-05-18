using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Wydawca
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Wydawca");
            Random r = new Random();
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
                for (int i = 0; i < 10; i++)
                {
                    var body = Encoding.UTF8.GetBytes($"wiadomość {i}");
                    string kanal;
                    if (i % 2 == 0)
                    {
                        kanal = "abc.def";
                    }
                    else
                    {
                        kanal = "abc.xyz";
                    }
                    channel.BasicPublish("abc", kanal, null, body);
                    Console.WriteLine($"Wysłano {i} - kanał - {kanal}");
                }
                Task.Run(() => Console.ReadKey()).Wait();
            }
        }
    }
}