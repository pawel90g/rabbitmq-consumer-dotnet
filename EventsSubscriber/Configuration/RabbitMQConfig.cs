using Microsoft.Extensions.Configuration;

namespace EventsSubscriber
{
    public class RabbitMQConfig
    {
        public string HostName { get; }
        public string UserName { get; }
        public string Password { get; }
        public RabbitMQConfig(IConfiguration configuration)
        {
            HostName = configuration["RabbitMQ:HostName"];
            UserName = configuration["RabbitMQ:UserName"];
            Password = configuration["RabbitMQ:Password"];
        }
    }
}