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
    internal class DeleteProductConfirmSubscriber : IConsumerHandler
    {
        private readonly ILogger<DeleteProductConfirmSubscriber> logger;
        private readonly IServiceScope serviceScope;
        private readonly IEventBusSubscriber consumer;

        public DeleteProductConfirmSubscriber(ILogger<DeleteProductConfirmSubscriber> logger, IServiceProvider serviceProvider
           , IEnumerable<IEventBusSubscriber> consumers, IEnumerable<IEventBusTopicPublisher> publishers)
        {
            this.logger = logger;
            this.serviceScope = serviceProvider.CreateScope();
            this.consumer = consumers.FirstOrDefault(s => s.SubscriberName.Equals("AddOrUpdateBidConfirm", StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task HandleMessageAsync(string message)
        {
            try
            {

                var product = JsonConvert.DeserializeObject<AuctionProduct>(message);

                if (product != null)
                {
                    var result = await this.serviceScope.ServiceProvider.GetRequiredService
                        <IRepository<AuctionProduct, string>>().DeleteAsync(product.Id);
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
