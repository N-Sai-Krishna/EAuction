using BidListing.Core.Domain;
using BidListing.Core.Domain.Messages;
using BidListing.Core.Services;
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

namespace BidListing.Core.Consumer
{
    internal class AddProductConsumer : IConsumerHandler
    {
        private readonly ILogger<AddProductConsumer> logger;
        private readonly IServiceScope serviceScope;
        private readonly IEventBusSubscriber consumer;

        public AddProductConsumer(ILogger<AddProductConsumer> logger, IServiceProvider serviceProvider
           , IEnumerable<IEventBusSubscriber> consumers, IEnumerable<IEventBusTopicPublisher> publishers)
        {
            this.logger = logger;
            this.serviceScope = serviceProvider.CreateScope();
            this.consumer = consumers.FirstOrDefault(s => s.SubscriberName.Equals("ProductAdded", StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task HandleMessageAsync(string message)
        {
            try
            {

                var product = JsonConvert.DeserializeObject<AuctionProduct>(message);

                if (product != null)
                {
                    var bidListingService = this.serviceScope.ServiceProvider.GetRequiredService<IBidListingService>();

                    await bidListingService.AddProduct(new ProductAndBidDetails
                    {
                        BidEndDate = product.BidEndDate,
                        Category = product.Category,
                        Id = product.Id,
                        ShortDescription = product.ShortDescription,
                        ProductName = product.ProductName,
                        StartingPrice = product.StartingPrice
                    });
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Consumer - ProductAdded - {ex.Message}");
            }
        }

        public async Task RegisterAsync()
        {
            this.consumer.Consume += this.HandleMessageAsync;
            await this.consumer.StartProcessingAsync();
        }
    }
}
