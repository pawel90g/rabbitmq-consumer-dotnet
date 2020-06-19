namespace ConsumerWorker.Services.Interfaces
{
    public interface IConsumerWorkerConfigProvider
    {
        string GetWorkerName();
        string GetQueueName();
    }
}