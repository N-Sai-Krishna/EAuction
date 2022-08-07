using EAuction.Common.Messaging;
using EAuction.Messaging.Interfaces;
using EAuction.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Seller.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seller.Core.Consumer
{
    internal class ValidateBidRequestSubscriber : IConsumerHandler
    {
        private readonly ILogger<ValidateBidRequestSubscriber> logger;
        private readonly IServiceScope serviceScope;
        private readonly IEventBusSubscriber consumer;
        private readonly IEventBusTopicPublisher eventBusPublisher;

        public ValidateBidRequestSubscriber(ILogger<ValidateBidRequestSubscriber> logger, IServiceProvider serviceProvider
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

                var product = JsonConvert.DeserializeObject<AuctionBidProduct>(message);

                if (product != null)
                {

                    var result = await this.serviceScope.ServiceProvider.GetRequiredService
                        <IRepository<AuctionProduct, string>>().FindByAsync(product.Id);
                    
                    if (result != null && result.BidEndDate > DateTime.Now.Date)
                    {
                        await this.eventBusPublisher.PublishMessageAsync(
                            new EventMessage()
                            {
                                MessageType = "AddOrUpdateBidConfim",
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
