namespace ConsumerWorker.Services.Interfaces
{
    public interface IConsumerWorkerConfigProvider
    {
        string GetQueueName();

        string GetExchangeName();
        string GetExchangeType();
        string GetRoutingKey();
        
        string GetWorkerName();
        bool UseExchange();
    }
}