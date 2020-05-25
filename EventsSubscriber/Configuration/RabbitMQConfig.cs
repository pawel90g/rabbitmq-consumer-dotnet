namespace EventsSubscriber
{
    public class RabbitMQConfig
    {
        public bool Enabled { get; set; }
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public ExchangeConfig ExchangeConfig { get; set; }
    }
}