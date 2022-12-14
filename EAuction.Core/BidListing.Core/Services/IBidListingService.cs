using BidListing.Core.Domain;
using BidListing.Core.Domain.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BidListing.Core.Services
{
    public interface IBidListingService
    {
        Task AddProduct(ProductAndBidDetails productAndBidDetails);
        
        Task AddUpdateBids(ProductAndBidDetails productAndBidDetails);

        Task<ProductAndBidDetails> GetProductAndBidDetails(string productId);

        Task DeleteProduct(AuctionProduct auctionProduct);
    }
}
