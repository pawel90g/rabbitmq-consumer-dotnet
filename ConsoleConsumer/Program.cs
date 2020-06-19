using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsSubscriber;
using EventsSubscriber.Interfaces;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ConsoleConsumer
{
    class Program
    {
        static IServiceProvider serviceProvider;

        static void Main(string[] args)
        {
            serviceProvider = Bootstrapper.GetServiceProvider();

            var queue = args[0] ?? throw new Exception("Missing first argument: queueName");

            var eventBusSubscriber = (IEventBusSubscriber)serviceProvider.GetService(typeof(IEventBusSubscriber));

            eventBusSubscriber.Subscribe(queue, (message) =>
            {
                Console.WriteLine(message);
            });

            // Subscribe(queue);

            // var factory = new ConnectionFactory() { HostName = "localhost", Password = "ZAQ!2wsx", UserName = "fft_test" };
            // using(var connection = factory.CreateConnection())
            // using(var channel = connection.CreateModel())
            // {
            //     channel.QueueDeclare(queue: queue,
            //                         durable: false,
            //                         exclusive: false,
            //                         autoDelete: false,
            //                         arguments: null);

            //     var consumer = new EventingBasicConsumer(channel);
            //     consumer.Received += (model, ea) =>
            //     {
            //         var body = ea.Body.ToArray();
            //         var message = Encoding.UTF8.GetString(body.ToArray());
            //         Console.WriteLine(" [x] Received {0}", message);
            //     };
            //     channel.BasicConsume(queue: queue,
            //                         autoAck: true,
            //                         consumer: consumer);

            //     Console.WriteLine(" Press [enter] to exit.");
            //     Console.ReadLine();
            // }
        }

        // static void Subscribe(string queue)
        // {
        //     var factory = new ConnectionFactory() { HostName = "localhost", Password = "ZAQ!2wsx", UserName = "fft_test" };
        //     using (var connection = factory.CreateConnection())
        //     using (var channel = connection.CreateModel())
        //     {
        //         channel.QueueDeclare(queue: queue,
        //                              durable: false,
        //                              exclusive: false,
        //                              autoDelete: false,
        //                              arguments: null);

        //         var consumer = new EventingBasicConsumer(channel);
        //         consumer.Received += (model, ea) =>
        //         {
        //             var body = ea.Body.ToArray();
        //             var message = Encoding.UTF8.GetString(body.ToArray());
        //             Console.WriteLine(" [x] Received {0}", message);
        //         };
        //         channel.BasicConsume(queue: queue,
        //                              autoAck: true,
        //                              consumer: consumer);
        //     }
        // }
    }
}
