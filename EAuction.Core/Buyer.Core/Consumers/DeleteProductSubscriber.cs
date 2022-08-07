using Buyer.Core.Domain.Messages;
using Buyer.Core.Repositories;
using EAuction.Common.Messaging;
using EAuction.Messaging.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buyer.Core.Consumers
{
    internal class DeleteProductSubscriber : IConsumerHandler
    {
        private readonly ILogger<DeleteProductSubscriber> logger;
        private readonly IServiceScope serviceScope;
        private readonly IEventBusSubscriber consumer;
        private readonly IEventBusTopicPublisher eventBusPublisher;

        public DeleteProductSubscriber(ILogger<DeleteProductSubscriber> logger, IServiceProvider serviceProvider
            , IEnumerable<IEventBusSubscriber> consumers, IEnumerable<IEventBusTopicPublisher> publishers)
        {
            this.logger = logger;
            this.serviceScope = serviceProvider.CreateScope();
            this.consumer = consumers.FirstOrDefault(s => s.SubscriberName.Equals("AddOrUpdateBidConfirm", StringComparison.InvariantCultureIgnoreCase));
            this.eventBusPublisher = publishers.FirstOrDefault(s => s.TopicName.Equals("eauctionmanagement", StringComparison.InvariantCultureIgnoreCase));
        }


        public async Task HandleMessageAsync(string message)
        {
            try
            {

                var product = JsonConvert.DeserializeObject<AuctionProduct>(message);

                if (product != null)
                {

                    var result = this.serviceScope.ServiceProvider.GetRequiredService<IBidRepository>().Query().Where(s => s.ProductId == product.Id).Count();

                    if (result == 0)
                    {
                        await this.eventBusPublisher.PublishMessageAsync(
                            new EventMessage()
                            {
                                MessageType = "ProductDetailConfirmation",
                                Message = message
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Consumer - DeleteProduct - {ex.Message}");

            }
        }

        public async Task RegisterAsync()
        {
            this.consumer.Consume += this.HandleMessageAsync;
            await this.consumer.StartProcessingAsync();
        }
    }
}
