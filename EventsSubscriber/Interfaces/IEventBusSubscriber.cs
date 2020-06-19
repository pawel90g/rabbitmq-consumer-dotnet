using System;
using System.Threading.Tasks;
using EventsSubscriber.Models.Abstract;

namespace EventsSubscriber.Interfaces
{
    public interface IEventBusSubscriber : IDisposable
    {
        void SubscribeQueue(string queue, Action<string> messageProcessor);
        void SubscribeExchange(string exchangeName, string routingKey, string exchangeType, Action<string> messageProcessor);
        void SubscribeExchange<TEvent>(string routingKey, string exchangeType, Action<TEvent> messageProcessor)
            where TEvent : EventBase;

        Task SubscribeQueueAsync(string queue, Action<string> messageProcessor);
        Task SubscribeExchangeAsync(string exchangeName, string routingKey, string exchangeType, Action<string> messageProcessor);
        Task SubscribeExchangeAsync<TEvent>(string routingKey, string exchangeType, Action<TEvent> messageProcessor)
            where TEvent : EventBase;
    }
}