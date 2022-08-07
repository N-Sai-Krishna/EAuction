using BidListing.Core.Domain;
using EAuction.Persistence;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
                await this.repository.AddAsync(productAndBidDetails);
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
                entity.Bids = productAndBidDetails.Bids;
                await this.repository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"BidListingService - AddUpdateBids - {ex.Message}");
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
