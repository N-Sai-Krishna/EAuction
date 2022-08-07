using EAuction.Messaging.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace EAuction.Messaging.Consumers
{
    internal class EventBusQueueConsumer : IEventBusQueueConsumer
    {
        private readonly ServiceBusProcessor processor;

        public EventBusQueueConsumer(ServiceBusClient client, string queueName)
        {
            this.QueueName = queueName;
            processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions());
            processor.ProcessMessageAsync += MessageHandler;
            processor.ProcessErrorAsync += ErrorHandler;
        }

        private Task ErrorHandler(ProcessErrorEventArgs arg)
        {
            return Task.CompletedTask;
        }

        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            if (this.Consume != null)
            {
                await this.Consume(body);
            }
        }

        public string QueueName { get; set; }

        public event Func<string, Task> Consume;

        public async Task StartProcessingAsync()
        {
            await processor.StartProcessingAsync();
        }
    }
}
