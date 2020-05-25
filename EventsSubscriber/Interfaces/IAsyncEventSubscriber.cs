using System.Threading.Tasks;
using EventsSubscriber.Models.Abstract;

namespace EventsSubscriber.Interfaces
{
    public interface IAsyncEventSubscriber
    {
        Task SubscribeAsync<TEvent>(string severity)
            where TEvent : EventBase;
    }
}