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
    internal class BidAddedOrUpdatedConsumer : IConsumerHandler
    {
        private readonly ILogger<BidAddedOrUpdatedConsumer> logger;
        private readonly IServiceScope serviceScope;
        private readonly IEventBusSubscriber consumer;

        public BidAddedOrUpdatedConsumer(ILogger<BidAddedOrUpdatedConsumer> logger, IServiceProvider serviceProvider
           , IEnumerable<IEventBusSubscriber> consumers, IEnumerable<IEventBusTopicPublisher> publishers)
        {
            this.logger = logger;
            this.serviceScope = serviceProvider.CreateScope();
            this.consumer = consumers.FirstOrDefault(s => s.SubscriberName.Equals("BidAddedOrUpdated", StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task HandleMessageAsync(string message)
        {
            try
            {
                var bidAddOrUpdateMessage = JsonConvert.DeserializeObject<BidAddOrUpdateMessage>(message);

                if (bidAddOrUpdateMessage != null)
                {
                    var bidListingService = this.serviceScope.ServiceProvider.GetRequiredService<IBidListingService>();

                    await bidListingService.AddProduct(new ProductAndBidDetails
                    {
                        Id = bidAddOrUpdateMessage.ProductId,
                        Bids = bidAddOrUpdateMessage.AuctionBuyerBidDetails.Select(
                        s => new BidDetails(){
                            BidAmount = s.BidAmount,
                            Email = s.Email,
                            FirstName = s.FirstName,
                            Phone = s.Phone
                        }).ToList()
                    });
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Consumer - BidAddedOrUpdated - {ex.Message}");
            }
        }

        public async Task RegisterAsync()
        {
            this.consumer.Consume += this.HandleMessageAsync;
            await this.consumer.StartProcessingAsync();
        }
    }
}
