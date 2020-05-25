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

        public ExchangeConfig GetExchangeConfig() => config.ExchangeConfig;

        public string GetHostName() => config.HostName;

        public string GetPassword() => config.Password;

        public string GetUserName() => config.UserName;

        public bool IsEnabled() => config.Enabled;
    }
}