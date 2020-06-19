using EventsSubscriber.Interfaces;
using Microsoft.Extensions.Configuration;
using  Microsoft.Extensions.DependencyInjection;

namespace EventsSubscriber.IoC
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection RegisterEventsSubscriber(
            this IServiceCollection service, 
            IConfiguration configuration)
        {
            service.AddSingleton<IRabbitConfigurationProvider>(
                new RabbitConfigurationProvider(
                    new RabbitMQConfig(configuration)));
            service.AddSingleton<IRabbitConnection, RabbitConnection>();
            service.AddSingleton<IEventBusSubscriber, EventBusSubscriber>();

            return service;
        }
    }
}