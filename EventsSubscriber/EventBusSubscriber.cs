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
        private readonly IRabbitConnection rabbitConnection;
        private readonly ILogger<EventBusSubscriber> logger;

        public EventBusSubscriber(
            IRabbitConnection rabbitConnection,
            ILogger<EventBusSubscriber> logger
            )
        {
            this.logger = logger;
            this.rabbitConnection = rabbitConnection;
        }

        public void SubscribeQueue(string queue, Action<string> messageProcessor)
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
                    var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                    messageProcessor(message);
                };
                channel.BasicConsume(queue: queue,
                                     autoAck: true,
                                     consumer: consumer);
                                     
                Console.ReadLine();
            }
        }

        public void SubscribeExchange(string exchangeName, string routingKey, string exchangeType, Action<string> messageProcessor)
        {
            var channel = rabbitConnection.GetChannel();

            channel.ExchangeDeclare(exchangeName, exchangeType, durable: false, autoDelete: false, arguments: null);
            var queueName = channel.QueueDeclare().QueueName;

            channel.QueueBind(queue: queueName,
                          exchange: exchangeName,
                          routingKey: routingKey);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                messageProcessor(message);
            };

            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }

        public void SubscribeExchange<TEvent>(string routingKey, string exchangeType, Action<TEvent> messageProcessor)
            where TEvent : EventBase
        {
            var exchangeName = GetExchangeName<TEvent>();
            var channel = rabbitConnection.GetChannel();

            channel.ExchangeDeclare(exchangeName, exchangeType, durable: false, autoDelete: false, arguments: null);
            var queueName = channel.QueueDeclare().QueueName;

            channel.QueueBind(queue: queueName,
                          exchange: exchangeName,
                          routingKey: routingKey);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = DeserializeEvent<TEvent>(ea.Body.ToArray());
                messageProcessor(body);
            };

            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }

        public Task SubscribeQueueAsync(string queue, Action<string> messageProcessor)
        {
            return Task.Run(() =>
            {
                try
                {
                    SubscribeQueue(queue, messageProcessor);
                }
                catch (Exception ex)
                {
                    logger.LogError("Error occures during event subscribing", ex);
                }
            });
        }

        public Task SubscribeExchangeAsync(string exchangeName, string routingKey, string exchangeType, Action<string> messageProcessor)
        {
            return Task.Run(() =>
            {
                try
                {
                    SubscribeExchange(exchangeName, routingKey, exchangeType, messageProcessor);
                }
                catch (Exception ex)
                {
                    logger.LogError("Error occures during event subscribing", ex);
                }
            });
        }

        public Task SubscribeExchangeAsync<TEvent>(string routingKey, string exchangeType, Action<TEvent> messageProcessor)
            where TEvent : EventBase
        {
            return Task.Run(() =>
            {
                try
                {
                    SubscribeExchange<TEvent>(routingKey, exchangeType, messageProcessor);
                }
                catch (Exception ex)
                {
                    logger.LogError("Error occures during event subscribing", ex);
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

        public void Dispose() => rabbitConnection?.Dispose();
    }
}