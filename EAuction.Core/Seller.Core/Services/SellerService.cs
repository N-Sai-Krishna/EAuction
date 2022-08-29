using EAuction.Messaging.Interfaces;
using EAuction.Persistence;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Seller.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EAuction.Messaging;
using EAuction.Common.Messaging;

namespace Seller.Core.Services
{
    internal class SellerService : ISellerService
    {
        private readonly ILogger<SellerService> logger;
        private readonly IRepository<AuctionProduct, string> productRepository;
        private readonly IRepository<AuctionProductSeller, string> sellerRepository;
        private readonly IEventBusTopicPublisher eventBusPublisher;

        public SellerService(ILogger<SellerService> logger, IRepository<AuctionProduct, string> productRepository, IRepository<AuctionProductSeller, string> sellerRepository
            , IEnumerable<IEventBusTopicPublisher> publishers)
        {
            this.logger = logger;
            this.productRepository = productRepository;
            this.sellerRepository = sellerRepository;
            this.eventBusPublisher = publishers.FirstOrDefault(s => s.TopicName.ToLower().Equals("eauctionmanagementsbtopic", StringComparison.InvariantCultureIgnoreCase));
        }
        public async Task<bool> AddProductAsync(AuctionProduct auctionProduct, AuctionProductSeller auctionProductSeller)
        {
            try
            {
                AuctionProductSeller seller = await this.sellerRepository.FindByAsync(auctionProductSeller.Phone);
                if (seller == null)
                {
                    seller = await this.sellerRepository.AddAsync(auctionProductSeller);
                }

                auctionProduct.SellerId = auctionProductSeller.Id = seller.Id;
                await this.productRepository.AddAsync(auctionProduct);

                await this.eventBusPublisher.PublishMessageAsync(
                    new EventMessage()
                    {
                        MessageType = "ProductAdded",
                        Message = JsonConvert.SerializeObject(auctionProduct)
                    });
            }
            catch (Exception ex)
            {
                this.logger.LogError($"SellerService - AddProductAsync - {ex.Message}");
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteProductFromAuctionSync(string productId)
        {
            try
            {
                AuctionProduct result = await this.productRepository.FindByAsync(productId);

                if (result != null && result.BidEndDate > DateTime.Now.Date)
                {

                    await this.productRepository.DeleteAsync(productId);

                    await this.eventBusPublisher.PublishMessageAsync(
                        new EventMessage()
                        {
                            MessageType = "ProductDeleteRequest",
                            Message = JsonConvert.SerializeObject(result)
                        });
                }
                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"SellerService - ProductDeleteRequest - {ex.Message}");
                return false;
            }
        }
    }
}
