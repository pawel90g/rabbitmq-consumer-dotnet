using System;
using System.Threading.Tasks;
using EventsSubscriber.Models.Abstract;

namespace EventsSubscriber.Interfaces
{
    public interface IEventBusSubscriber : IDisposable
    {
        void Subscribe(string queue, Action<string> messageProcessor);
        void Subscribe<TEvent>(string routingKey, Action<TEvent> messageProcessor)
            where TEvent : EventBase;

        Task SubscribeAsync(string queue, Action<string> messageProcessor);
        Task SubscribeAsync<TEvent>(string routingKey, Action<TEvent> messageProcessor)
            where TEvent : EventBase;
    }
}