using System;
using RabbitMQ.Client;

namespace EventsSubscriber.Interfaces
{
    public interface IRabbitConnection : IDisposable
    {
        IConnection GetConnection();
        IModel GetChannel();
        void Close();
    }
}