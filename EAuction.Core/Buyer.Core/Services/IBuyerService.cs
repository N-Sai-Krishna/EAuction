using Buyer.Core.Domain;
using Buyer.Core.Domain.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Buyer.Core.Services
{
    public interface IBuyerService
    {
        Task<bool> AddBidAsync(AuctionBuyer auctionBuyer, AuctionBid auctionBid);

        Task<bool> UpdateBidAsync(string phoneNumber, string productId, decimal bidAmount);

        Task<IList<AuctionBuyerBidDetails>> GetBidsForProduct(string productId);

    }
}
