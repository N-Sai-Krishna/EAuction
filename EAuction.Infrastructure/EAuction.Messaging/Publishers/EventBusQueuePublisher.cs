using Azure.Messaging.ServiceBus;
using EAuction.Common.Messaging;
using EAuction.Messaging.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAuction.Messaging.Publishers
{
    internal class EventBusQueuePublisher : IEventBusQueuePublisher
    {
        private readonly ServiceBusSender sender;
        public EventBusQueuePublisher(ServiceBusClient client, string queueName)
        {
            this.QueueName = queueName;
            this.sender = client.CreateSender(queueName);
        }

        public string QueueName { get; private set; }

        public async Task PublishMessageAsync(EventMessage messageToPublish)
        {
            await this.sender.SendMessageAsync(new ServiceBusMessage(messageToPublish.Message) { Subject = messageToPublish.MessageType});
        }
    }
}
