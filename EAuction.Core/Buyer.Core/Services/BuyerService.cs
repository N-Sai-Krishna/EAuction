using Buyer.Core.Domain;
using Buyer.Core.Domain.Messages;
using Buyer.Core.Repositories;
using EAuction.Common.Messaging;
using EAuction.Core.Common.Exceptions;
using EAuction.Messaging.Interfaces;
using EAuction.Persistence;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buyer.Core.Services
{
    internal class BuyerService : IBuyerService
    {
        private readonly ILogger<BuyerService> logger;
        private readonly IRepository<AuctionBuyer, string> buyerRepository;
        private readonly IBidRepository bidRepository;
        private readonly IEventBusTopicPublisher eventBusPublisher;

        public BuyerService(ILogger<BuyerService> logger, IRepository<AuctionBuyer, string> buyerRepository, IBidRepository bidRepository
            , IEnumerable<IEventBusTopicPublisher> publishers)
        {
            this.logger = logger;
            this.buyerRepository = buyerRepository;
            this.bidRepository = bidRepository;
            this.eventBusPublisher = publishers.FirstOrDefault(s => s.TopicName.ToLower().Equals("eauctionmanagementsbtopic", StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<bool> AddBidAsync(AuctionBuyer auctionBuyer, AuctionBid auctionBid)
        {
            try
            {
                AuctionBuyer buyer = await this.FindOrAddBuyer(auctionBuyer);
                var bid = await this.bidRepository.FindBidByAsync(auctionBid.ProductId, buyer.Id);

                if (bid != null)
                {
                    throw new EAuctionDomainException($"Bid already exists - productId: {auctionBid.ProductId}, buyerPhone:{auctionBuyer.Phone}");
                }

                auctionBid.BuyerId = buyer.Id;

                await FindOrAddAuctionBid(auctionBid);

                BidAddOrUpdateMessage bidAddOrUpdateMessage = new BidAddOrUpdateMessage
                {
                    ProductId = auctionBid.ProductId,
                    AuctionBuyerBidDetails = new List<AuctionBuyerBidDetails>
                {
                     new AuctionBuyerBidDetails()
                {
                    ProductId = auctionBid.ProductId,
                    FirstName = buyer.FirstName,
                    Phone = buyer.Phone,
                    Email = buyer.Email,
                    BidAmount = auctionBid.BidAmount
                }}};

                await this.eventBusPublisher.PublishMessageAsync(
                    new EventMessage()
                    {
                        MessageType = "BidAddedOrUpdated",
                        Message = JsonConvert.SerializeObject(bidAddOrUpdateMessage)

                    });
            }
            catch (Exception ex)
            {
                this.logger.LogError($"BuyerService - AddBidAsync - {ex.Message}");
                return false;
            }

            return true;
        }

        private async Task<AuctionBuyer> FindOrAddBuyer(AuctionBuyer auctionBuyer)
        {
            var result = await this.buyerRepository.FindByAsync(auctionBuyer.Phone);
            if (result == null)
            {
                result = await this.buyerRepository.AddAsync(auctionBuyer);
            }
            return result;
        }

        private async Task<AuctionBid> FindOrAddAuctionBid(AuctionBid auctionBid)
        {

            var result = await bidRepository.FindBidByAsync(auctionBid.ProductId, auctionBid.BuyerId);
            if (result == null)
            {
                await bidRepository.AddAsync(auctionBid);
            }
            else
            {
                await bidRepository.UpdateAsync(new AuctionBid()
                {
                    BuyerId = result.BuyerId,
                    BidAmount = auctionBid.BidAmount,
                    Id = result.Id,
                    ProductId = result.ProductId
                });
            }

            return result;
        }


        public async Task<IList<AuctionBuyerBidDetails>> GetBidsForProduct(string productId)
        {
            try
            {
                return await Task.Run(() =>
                {
                    return this.bidRepository.Query().AsEnumerable()
                        .Join(this.buyerRepository.Query().AsEnumerable(), s => s.BuyerId, k => k.Id,
                        (bid, buyer) => new AuctionBuyerBidDetails()
                        {
                            ProductId = bid.ProductId,
                            BidAmount = bid.BidAmount,
                            Email = buyer.Email,
                            FirstName = buyer.FirstName,
                            Phone = buyer.Phone
                        }).Where(s => s.ProductId == productId).OrderByDescending(s => s.BidAmount).ToList();
                });
            }
            catch (Exception ex)
            {
                this.logger.LogError($"BuyerService - GetBidsForProduct - {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateBidAsync(string phoneNumber, string productId, decimal bidAmount)
        {
            try
            {
                var auctionBuyer = await this.buyerRepository.FindByAsync(phoneNumber);

                if (auctionBuyer == null)
                {
                    return false; 
                }

                var auctionBid = await this.bidRepository.FindBidByAsync(productId, auctionBuyer.Id);

                auctionBid.BidAmount = bidAmount;

                await this.bidRepository.UpdateAsync(auctionBid);


                BidAddOrUpdateMessage bidAddOrUpdateMessage = new BidAddOrUpdateMessage
                {
                    ProductId = auctionBid.ProductId,
                    AuctionBuyerBidDetails = new List<AuctionBuyerBidDetails>
                {
                     new AuctionBuyerBidDetails()
                {
                    ProductId = auctionBid.ProductId,
                    FirstName = auctionBuyer.FirstName,
                    Phone = auctionBuyer.Phone,
                    Email = auctionBuyer.Email,
                    BidAmount = auctionBid.BidAmount
                }}
                };

                await this.eventBusPublisher.PublishMessageAsync(
                    new EventMessage()
                    {
                        MessageType = "BidAddedOrUpdated",
                        Message = JsonConvert.SerializeObject(bidAddOrUpdateMessage)

                    });
            }
            catch (Exception ex)
            {
                this.logger.LogError($"BuyerService - UpdateBidAsync - {ex.Message}");
                return false;
            }

            return true;
        }
    }
}
