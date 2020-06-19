using System.Linq;
using ConsumerWorker.Services;
using ConsumerWorker.Services.Interfaces;
using EventsSubscriber.IoC;
using EventsSubscriber.Models.Enums;
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

                    var exchangeName = argsList.FirstOrDefault(a => a.StartsWith("exchange"))?.Split("=").ElementAtOrDefault(1);
                    var exchangeType = ExchangeType.Parse(argsList.FirstOrDefault(a => a.StartsWith("exchange"))?.Split("=").ElementAtOrDefault(1));
                    var routingKey = ExchangeType.Parse(argsList.FirstOrDefault(a => a.StartsWith("routingKey"))?.Split("=").ElementAtOrDefault(1));

                    var workerName = argsList.FirstOrDefault(a => a.StartsWith("worker"))?.Split("=").ElementAtOrDefault(1) ?? "Noname";

                    var cfgProvider = string.IsNullOrEmpty(exchangeName) || string.IsNullOrEmpty(routingKey)
                        ? new ConsumerWorkerConfigProvider(queueName,
                            workerName)
                        : new ConsumerWorkerConfigProvider(exchangeName,
                            exchangeType,
                            routingKey,
                            workerName);

                    services.AddSingleton<IConsumerWorkerConfigProvider>(cfgProvider);
                    services.RegisterEventsSubscriber(hostContext.Configuration);
                    services.AddHostedService<QueueConsumerService>();
                });
    }
}
