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
    internal class ProductDeleteRequestConsumer : IConsumerHandler
    {
        private readonly ILogger<ProductDeleteRequestConsumer> logger;
        private readonly IServiceScope serviceScope;
        private readonly IEventBusSubscriber consumer;
        private readonly IEventBusTopicPublisher eventBusPublisher;

        public ProductDeleteRequestConsumer(ILogger<ProductDeleteRequestConsumer> logger, IServiceProvider serviceProvider
            , IEnumerable<IEventBusSubscriber> consumers, IEnumerable<IEventBusTopicPublisher> publishers)
        {
            this.logger = logger;
            this.serviceScope = serviceProvider.CreateScope();
            this.consumer = consumers.FirstOrDefault(s => s.SubscriberName.Equals("ProductDeleteRequest", StringComparison.InvariantCultureIgnoreCase));
            this.eventBusPublisher = publishers.FirstOrDefault(s => s.TopicName.Equals("eauctionmanagementsbtopic", StringComparison.InvariantCultureIgnoreCase));
        }


        public async Task HandleMessageAsync(string message)
        {
            try
            {

                var product = JsonConvert.DeserializeObject<AuctionProduct>(message);

                if (product != null)
                {

                    var bidRepository = this.serviceScope.ServiceProvider.GetRequiredService<IBidRepository>();

                    var bids = await bidRepository.FindBidByProductIdAsync(product.Id);

                    foreach (var bid in bids)
                    {
                      await bidRepository.DeleteAsync(bid.Id);
                    }


                    if (bids.Any())
                    {
                        await this.eventBusPublisher.PublishMessageAsync(
                            new EventMessage()
                            {
                                MessageType = "ProductDeleteConfirmation",
                                Message = JsonConvert.SerializeObject(product)
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
