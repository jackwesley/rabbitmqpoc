using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rabbitmqreceive
{
    class Program
    {
        private static List<int> _pares = new List<int>();
        private static List<int> _impares = new List<int>();
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

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received +=  Consumer_Received;

                channel.BasicConsume(queue: "Numbers",
                                    autoAck: true,
                                    consumer: consumer);

            }


            PrintEvensAndOdds();
            Console.WriteLine("\n Press any key to exit");
            Console.ReadKey();
        }


        private static void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {

            var message = Encoding.UTF8.GetString(e.Body);
            var number = int.Parse(message);
            if (number < 100)
            {
                if (number % 2 == 0)
                    _pares.Add(number);
                else
                    _impares.Add(number);
            }
            else
            {
                Console.WriteLine($"\n Numero maior que 100 encontrado : {number}");
            }
        }


        private static void PrintEvensAndOdds()
        {
            Console.WriteLine("\n Números pares encontrados: ");
            foreach (int par in _pares)
            {
                Console.WriteLine(par.ToString() + ",");
            }

            Console.WriteLine("\n Números ímpares encontrados: ");
            foreach (int impar in _impares)
            {
                Console.WriteLine(impar.ToString() + ",");
            }
        }
    }
}
