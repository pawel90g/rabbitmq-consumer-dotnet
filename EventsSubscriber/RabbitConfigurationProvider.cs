using EventsSubscriber.Interfaces;

namespace EventsSubscriber
{
    public class RabbitConfigurationProvider : IRabbitConfigurationProvider
    {
        private readonly RabbitMQConfig config;
        public RabbitConfigurationProvider(RabbitMQConfig config)
        {
            this.config = config;
        }

        public string GetHostName() => config.HostName;

        public string GetPassword() => config.Password;

        public string GetUserName() => config.UserName;
    }
}