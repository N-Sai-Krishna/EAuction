using Buyer.Core.Domain;
using EAuction.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Buyer.Core.Repositories
{
    public interface IBidRepository :IRepository<AuctionBid, string>
    {
        Task<AuctionBid> FindBidByAsync(string productId, string buyerId);

        Task<List<AuctionBid>> FindBidByProductIdAsync(string productId);
    }
}
