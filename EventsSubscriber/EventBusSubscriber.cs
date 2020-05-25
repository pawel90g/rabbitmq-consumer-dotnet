using System;
using System.Text;
using System.Threading.Tasks;
using EventsSubscriber.Interfaces;
using EventsSubscriber.Models.Abstract;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EventsSubscriber
{
    public class EventBusSubscriber : IAsyncEventSubscriber
    {
        private readonly IRabbitConfigurationProvider rabbitConfigurationProvider;
        private readonly IRabbitConnection rabbitConnection;
        private readonly ILogger<EventBusSubscriber> logger;

        public EventBusSubscriber(
            IRabbitConfigurationProvider rabbitConfigurationProvider,
            IRabbitConnection rabbitConnection,
            ILogger<EventBusSubscriber> logger
            )
        {
            this.logger = logger;
            this.rabbitConfigurationProvider = rabbitConfigurationProvider;
            this.rabbitConnection = rabbitConnection;
        }

        public async Task SubscribeAsync<TEvent>(string severity)
            where TEvent : EventBase
        {
            if (!rabbitConfigurationProvider.IsEnabled())
            {
                logger.LogInformation("RabbitMQ integration disabled");
                return;
            }

            var exchangeCfg = rabbitConfigurationProvider.GetExchangeConfig();


            await Task.Run(() =>
            {
                try
                {
                    var exchangePrefix = string.IsNullOrEmpty(exchangeCfg.NamePrefix) ? "" : $"{exchangeCfg.NamePrefix}_";
                    var exchangeName = $"{exchangePrefix}{GetExchangeName<TEvent>()}";
                    var channel = rabbitConnection.GetChannel();

                    channel.ExchangeDeclare(exchangeName, "direct", durable: false, autoDelete: false, arguments: null);
                    var queueName = channel.QueueDeclare().QueueName;

                    channel.QueueBind(queue: queueName,
                                  exchange: exchangeName,
                                  routingKey: severity);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = DeserializeEvent<TEvent>(ea.Body.ToArray());
                        var routingKey = ea.RoutingKey;
                    };

                    channel.BasicConsume(queue: queueName,
                                         autoAck: true,
                                         consumer: consumer);
                }
                catch (Exception ex)
                {
                    logger.LogError("Error occures during event publishing", ex);
                }
            });
        }

        private byte[] SerializeEvent(object @object) =>
            Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@object));

        private TEvent DeserializeEvent<TEvent>(byte[] bytes)
            where TEvent : EventBase =>
            JsonConvert.DeserializeObject<TEvent>(Encoding.UTF8.GetString(bytes));

        private static string GetExchangeName(EventBase @event) => @event.GetType().FullName;
        private static string GetExchangeName<TEvent>() where TEvent : EventBase => typeof(TEvent).FullName;
    }
}