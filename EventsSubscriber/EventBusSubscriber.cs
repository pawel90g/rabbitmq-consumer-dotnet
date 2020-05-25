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
    public class EventBusSubscriber : IEventBusSubscriber
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

        public void Subscribe(string queue, string routingKey)
        {
            using (var channel = rabbitConnection.GetChannel())
            {
                channel.QueueDeclare(queue: queue,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                };
                channel.BasicConsume(queue: queue,
                                     autoAck: true,
                                     consumer: consumer);
            }
        }

        public void Subscribe<TEvent>(string routingKey)
            where TEvent : EventBase
        {
            var exchangeCfg = rabbitConfigurationProvider.GetExchangeConfig();

            var exchangeName = GetExchangeName<TEvent>();
            var channel = rabbitConnection.GetChannel();

            channel.ExchangeDeclare(exchangeName, "direct", durable: false, autoDelete: false, arguments: null);
            var queueName = channel.QueueDeclare().QueueName;

            channel.QueueBind(queue: queueName,
                          exchange: exchangeName,
                          routingKey: routingKey);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = DeserializeEvent<TEvent>(ea.Body.ToArray());
            };

            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }

        public Task SubscribeAsync(string queue, string routingKey)
        {
            return Task.Run(() =>
            {
                try
                {
                    Subscribe(queue, routingKey);
                }
                catch (Exception ex)
                {
                    logger.LogError("Error occures during event publishing", ex);
                }
            });
        }

        public Task SubscribeAsync<TEvent>(string routingKey)
            where TEvent : EventBase
        {
            return Task.Run(() =>
            {
                try
                {
                    Subscribe<TEvent>(routingKey);
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