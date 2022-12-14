using Buyer.Core.Domain;
using Buyer.Core.Domain.Messages;
using Buyer.Core.Repositories;
using Buyer.Core.Services;
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
    internal class AddOrUpdateBidConsumer : IConsumerHandler
    {
        private readonly ILogger<AddOrUpdateBidConsumer> logger;
        private readonly IServiceScope serviceScope;
        private readonly IEventBusSubscriber consumers;
        private readonly IEventBusTopicPublisher eventBusPublisher;

        public AddOrUpdateBidConsumer(ILogger<AddOrUpdateBidConsumer> logger,IServiceProvider serviceProvider
            , IEnumerable<IEventBusSubscriber> consumers, IEnumerable<IEventBusTopicPublisher> publishers)
        {
            this.logger = logger;
            this.serviceScope = serviceProvider.CreateScope();
            this.consumers = consumers.FirstOrDefault(s=>s.SubscriberName.Equals("AddOrUpdateBid", StringComparison.InvariantCultureIgnoreCase));
            //this.consumers = consumers.FirstOrDefault(s=>s.SubscriberName.Equals("AddOrUpdateBidConfirm", StringComparison.InvariantCultureIgnoreCase));
            this.eventBusPublisher = publishers.FirstOrDefault(s => s.TopicName.Equals("eauctionmanagementsbtopic", StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task HandleMessageAsync(string message)
        {
            try
            {
                var bid = JsonConvert.DeserializeObject<AuctionBid>(message);

                if (bid != null)
                {
                    var bidRepository = this.serviceScope.ServiceProvider.GetRequiredService<IBidRepository>();
                    var buyerService = this.serviceScope.ServiceProvider.GetRequiredService<IBuyerService>();

                    var result = await bidRepository.FindBidByAsync(bid.ProductId, bid.BuyerId);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Consumer - AddOrUpdateBidConfirm - {ex.Message}");
               
            }
        }

        public async Task RegisterAsync()
        {
            this.consumers.Consume += this.HandleMessageAsync;
            await this.consumers.StartProcessingAsync();
        }
    }
}
