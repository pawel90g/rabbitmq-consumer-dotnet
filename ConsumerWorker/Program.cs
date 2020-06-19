using System.Linq;
using ConsumerWorker.Services;
using ConsumerWorker.Services.Interfaces;
using EventsSubscriber.IoC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ConsumerWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var argsList = args.ToList();
                    var queueName = argsList.FirstOrDefault(a => a.StartsWith("queue"))?.Split("=").ElementAtOrDefault(1);
                    var workerName = argsList.FirstOrDefault(a => a.StartsWith("worker"))?.Split("=").ElementAtOrDefault(1) ?? "Noname";

                    services.AddSingleton<IConsumerWorkerConfigProvider>(
                        new ConsumerWorkerConfigProvider(queueName, workerName));

                    services.RegisterEventsSubscriber(hostContext.Configuration);
                    services.AddHostedService<QueueConsumerService>();
                });
    }
}
