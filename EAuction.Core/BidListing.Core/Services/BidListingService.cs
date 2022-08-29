using BidListing.Core.Domain;
using BidListing.Core.Domain.Messages;
using BidListing.Core.Repositories;
using EAuction.Persistence;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BidListing.Core.Services
{
    internal class BidListingService : IBidListingService
    {
        private readonly IRepository<ProductAndBidDetails, string> repository;
        private readonly ILogger<BidListingService> logger;

        public BidListingService(IRepository<ProductAndBidDetails, string> repository, ILogger<BidListingService> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task AddProduct(ProductAndBidDetails productAndBidDetails)
        {
            try
            {
                var dbProduct = await this.repository.FindByAsync(productAndBidDetails.Id);
                if (dbProduct == null)
                {
                    await this.repository.AddAsync(productAndBidDetails);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"BidListingService - AddProduct - {ex.Message}");
            }
        }

        public async Task AddUpdateBids(ProductAndBidDetails productAndBidDetails)
        {
            try
            {
                var entity = await this.repository.FindByAsync(productAndBidDetails.Id);

                if (entity.Bids == null)
                {
                    entity.Bids = new List<BidDetails>();
                    entity.Bids.Add(productAndBidDetails.Bids.FirstOrDefault());
                }
                else
                {
                    var bid = entity.Bids.FirstOrDefault(s => s.Phone == productAndBidDetails.Bids.FirstOrDefault().Phone);

                    if (bid == null)
                    {
                        entity.Bids.Add(productAndBidDetails.Bids.FirstOrDefault());
                    }
                    else
                    {
                        bid.BidAmount = productAndBidDetails.Bids.FirstOrDefault().BidAmount;
                    }
                }
                await this.repository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"BidListingService - AddUpdateBids - {ex.Message}");
            }
        }

        public async Task DeleteProduct(AuctionProduct auctionProduct)
        {
            ProductAndBidDetails productAndBid = await this.repository.FindByAsync(auctionProduct.Id);

            if (productAndBid != null)
            {
                await this.repository.DeleteAsync(productAndBid.Id);
            }
        }

        public async Task<ProductAndBidDetails> GetProductAndBidDetails(string productId)
        {
            try
            {
                return await this.repository.FindByAsync(productId);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"BidListingService - GetProductAndBidDetails - {ex.Message}");
            }
            return null;
        }
    }
}
