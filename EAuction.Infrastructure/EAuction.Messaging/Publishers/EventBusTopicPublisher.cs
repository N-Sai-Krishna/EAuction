using EAuction.Common.Messaging;
using EAuction.Messaging.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace EAuction.Messaging.Publishers
{
    internal class EventBusTopicPublisher : IEventBusTopicPublisher
    {
        private readonly ServiceBusSender sender;

        public EventBusTopicPublisher(ServiceBusClient client, string topicName)
        {
            this.TopicName = topicName;
            this.sender = client.CreateSender(topicName);
                
        }

        public string TopicName { get; set; }


        public async Task PublishMessageAsync(EventMessage messageToPublish)
        {
            await this.sender.SendMessageAsync(new ServiceBusMessage(messageToPublish.Message) { Subject = messageToPublish.MessageType});
        }
    }
}
