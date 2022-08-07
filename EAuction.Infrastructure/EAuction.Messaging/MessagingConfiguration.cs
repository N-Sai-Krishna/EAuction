using EAuction.Common.Configurations;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Collections.Generic;
using EAuction.Messaging.Interfaces;
using EAuction.Messaging.Publishers;
using Azure.Messaging.ServiceBus;
using EAuction.Messaging.Consumers;

namespace EAuction.Messaging
{
    public static class MessagingConfiguration
    {
        public static void ConfigureMessaging(this IServiceCollection serviceCollection, ServiceBusSettings serviceBusSettings)
        {
            serviceCollection.AddSingleton<ServiceBusClient>((_) => new ServiceBusClient(serviceBusSettings.ConnectionString));
            CreateQueues(serviceCollection, serviceBusSettings.ServiceBusQueues, serviceBusSettings.ConnectionString);
            CreateTopicPublishers(serviceCollection, serviceBusSettings.ServiceBusTopics, serviceBusSettings.ConnectionString);
            CreateQueueConsumers(serviceCollection, serviceBusSettings.ServiceBusQueueConsumers, serviceBusSettings.ConnectionString);
            CreateTopicSubscribers(serviceCollection, serviceBusSettings.ServiceBusSubscribers, serviceBusSettings.ConnectionString);
        }

        public static void CreateQueues(IServiceCollection serviceCollection, IEnumerable<string> queues, string connectionString)
        {
            if (queues != null)
            {
                foreach (var queue in queues)
                {
                    serviceCollection.AddSingleton<IEventBusQueuePublisher>(provider =>
                    new EventBusQueuePublisher(provider.GetRequiredService<ServiceBusClient>(), queue));
                }
            }
        }

        public static void CreateTopicPublishers(IServiceCollection serviceCollection, IEnumerable<string> topics, string connectionString)
        {
            if (topics != null)
            {
                foreach (var topic in topics)
                {
                    serviceCollection.AddSingleton<IEventBusTopicPublisher>(provider =>
                    new EventBusTopicPublisher(provider.GetRequiredService<ServiceBusClient>(), topic));
                }
            }
        }

        public static void CreateQueueConsumers(IServiceCollection serviceCollection, IEnumerable<string> consumers, string connectionString)
        {
            if (consumers != null)
            {
                foreach (var consumer in consumers)
                {
                    serviceCollection.AddSingleton<IEventBusQueueConsumer>(provider =>
                    new EventBusQueueConsumer(provider.GetRequiredService<ServiceBusClient>(), consumer));
                }
            }
        }

        public static void CreateTopicSubscribers(IServiceCollection serviceCollection, IEnumerable<string> subscribers, string connectionString)
        {
            if (subscribers != null)
            {
                foreach (var subscriber in subscribers)
                {
                    var results = subscriber.Split(":");
                    serviceCollection.AddSingleton<IEventBusSubscriber>(provider =>
                    new EventBusSubscriber(provider.GetRequiredService<ServiceBusClient>(), results[0], results[1]));
                }
            }
        }
    }
}

