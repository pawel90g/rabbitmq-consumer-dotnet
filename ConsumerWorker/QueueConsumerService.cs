using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ConsumerWorker.Services.Interfaces;
using EventsSubscriber.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ConsumerWorker
{
    public class QueueConsumerService : IHostedService
    {
        private readonly ILogger<QueueConsumerService> _logger;
        private readonly IEventBusSubscriber eventBusSubscriber;
        private readonly IConsumerWorkerConfigProvider consumerWorkerConfigProvider;

        public QueueConsumerService(
            ILogger<QueueConsumerService> logger,
            IEventBusSubscriber eventBusSubscriber, 
            IConsumerWorkerConfigProvider consumerWorkerConfigProvider)
        {
            _logger = logger;
            this.eventBusSubscriber = eventBusSubscriber;
            this.consumerWorkerConfigProvider = consumerWorkerConfigProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Start");
            await eventBusSubscriber.SubscribeAsync(consumerWorkerConfigProvider.GetQueueName(), (message) => Console.WriteLine($"[Consumer {consumerWorkerConfigProvider.GetWorkerName()}] Message received: {message}"));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Stop");
            return Task.CompletedTask;
        }
    }
}
