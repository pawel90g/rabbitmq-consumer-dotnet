using EventsSubscriber.Interfaces;
using RabbitMQ.Client;

namespace EventsSubscriber 
{
    public class RabbitConnection : IRabbitConnection
    {
        private readonly IRabbitConfigurationProvider rabbitConfigurationProvider;

        private IConnection connection;

        public RabbitConnection(IRabbitConfigurationProvider rabbitConfigurationProvider)
        {
            this.rabbitConfigurationProvider = rabbitConfigurationProvider;
        }

        public void Close()
        {
            connection.Close();
        }

        public void Dispose()
        {
            if (connection is null) return;

            connection.Close();
            connection.Dispose();
        }

        public IModel GetChannel() => GetConnection().CreateModel();

        public IConnection GetConnection()
        {
            if (connection != null)
                return connection;

            var factory = new ConnectionFactory
            {
                HostName = rabbitConfigurationProvider.GetHostName(),
                UserName = rabbitConfigurationProvider.GetUserName(),
                Password = rabbitConfigurationProvider.GetPassword()
            };

            connection?.Dispose();
            connection = factory.CreateConnection();

            return connection;
        }
    }
}