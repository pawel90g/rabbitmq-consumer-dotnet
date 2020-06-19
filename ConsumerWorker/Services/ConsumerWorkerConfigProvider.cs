using ConsumerWorker.Services.Interfaces;

namespace ConsumerWorker.Services {
    public class ConsumerWorkerConfigProvider : IConsumerWorkerConfigProvider
    {
        private readonly string queueName;
        private readonly string workerName;

        public ConsumerWorkerConfigProvider(string queueName = "queue", string workerName = "Noname")
        {
            this.queueName = queueName;
            this.workerName = workerName;
        }

        public string GetQueueName() => queueName;
        public string GetWorkerName() => workerName;
    }
}