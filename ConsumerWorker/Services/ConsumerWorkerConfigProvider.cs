using ConsumerWorker.Services.Interfaces;
using EventsSubscriber.Models.Enums;

namespace ConsumerWorker.Services
{
    public class ConsumerWorkerConfigProvider : IConsumerWorkerConfigProvider
    {
        private readonly string queueName;

        private readonly string exchangeName;
        private readonly string exchangeType;
        private readonly string routingKey;

        private readonly string workerName;

        public ConsumerWorkerConfigProvider(
            string queueName = "queue",
            string workerName = null)
        {
            this.queueName = queueName;
            this.workerName = workerName;
        }

        public ConsumerWorkerConfigProvider(
                string exchangeName = "exchange",
                string exchangeType = ExchangeType.Direct,
                string routingKey = "route",
                string workerName = null)
        {
            this.exchangeName = exchangeName;
            this.exchangeType = exchangeType;
            this.routingKey = routingKey;
            this.workerName = workerName;
        }


        public string GetExchangeName() => exchangeName;

        public string GetExchangeType() => exchangeType;
        public string GetQueueName() => queueName;
        public string GetRoutingKey() => routingKey;

        public string GetWorkerName() => workerName;

        public bool UseExchange() => string.IsNullOrEmpty(queueName)
            && !string.IsNullOrEmpty(exchangeName);
    }
}