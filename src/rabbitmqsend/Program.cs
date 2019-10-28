using RabbitMQ.Client;
using System;
using System.Text;

namespace rabbitmqsend
{
    class Program
    {
        public static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "Numbers",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                for(int i = 0; i <= 10; i++)
                {
                    Random rdn = new Random();
                    var randonNum = rdn.Next(0, 150);

                    var body = Encoding.UTF8.GetBytes(randonNum.ToString());

                    channel.BasicPublish(exchange: "",
                                         routingKey: "Numbers",
                                         basicProperties: null,
                                         body: body);

                 Console.WriteLine($"\n Numero Gerado:  {randonNum} ");
                }
               

                Console.WriteLine("\n Finalizado!");
            }


            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
