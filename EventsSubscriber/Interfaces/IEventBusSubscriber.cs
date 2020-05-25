using System.Threading.Tasks;
using EventsSubscriber.Models.Abstract;

namespace EventsSubscriber.Interfaces
{
    public interface IEventBusSubscriber
    {
        void Subscribe(string queue, string routingKey);
        void Subscribe<TEvent>(string routingKey)
            where TEvent : EventBase;

        Task SubscribeAsync(string queue, string routingKey);
        Task SubscribeAsync<TEvent>(string routingKey)
            where TEvent : EventBase;
    }
}