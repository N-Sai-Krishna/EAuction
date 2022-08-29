using BidListing.Core.Domain;
using BidListing.Core.Domain.Messages;
using BidListing.Core.Repositories;
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
    internal class ProductDeleteConfirmationConsumer : IConsumerHandler
    {
        private readonly ILogger<ProductDeleteConfirmationConsumer> logger;
        private readonly IServiceScope serviceScope;
        private readonly IEventBusSubscriber consumer;

        public ProductDeleteConfirmationConsumer(ILogger<ProductDeleteConfirmationConsumer> logger, IServiceProvider serviceProvider
           , IEnumerable<IEventBusSubscriber> consumers)
        {
            this.logger = logger;
            this.serviceScope = serviceProvider.CreateScope();
            this.consumer = consumers.FirstOrDefault(s => s.SubscriberName.Equals("ProductDeleteConfirmation", StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task HandleMessageAsync(string message)
        {
            try
            {
                var product = JsonConvert.DeserializeObject<AuctionProduct>(message);

                if (product != null)
                {
                    var bidListingService = this.serviceScope.ServiceProvider.GetRequiredService<IBidListingService>();

                    await bidListingService.DeleteProduct(product);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Consumer - ProductDeleted - {ex.Message}");
            }
        }

        public async Task RegisterAsync()
        {
            this.consumer.Consume += this.HandleMessageAsync;
            await this.consumer.StartProcessingAsync();
        }
    }
}
