using EAuction.Messaging.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Azure.Messaging.ServiceBus;
using System.Threading.Tasks;

namespace EAuction.Messaging.Consumers
{
    internal class EventBusSubscriber : IEventBusSubscriber
    {
        private readonly ServiceBusProcessor processor;

        public EventBusSubscriber(ServiceBusClient client, string topicName, string subscriberName)
        {
            this.SubscriberName = subscriberName;
            this.processor = client.CreateProcessor(topicName, subscriberName);
            this.processor.ProcessMessageAsync += this.MessageHandler;
            this.processor.ProcessErrorAsync += this.ErrorHandler;
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

        public string SubscriberName { get; set; }

        public event Func<string, Task> Consume;

        public async Task StartProcessingAsync()
        {
            await processor.StartProcessingAsync();
        }
    }
}
