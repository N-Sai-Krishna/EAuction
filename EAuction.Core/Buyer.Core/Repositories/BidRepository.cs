using Buyer.Core.Domain;
using EAuction.Persistence;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Buyer.Core.Repositories
{
    internal class BidRepository : Repository<AuctionBid, string>, IBidRepository
    {
        public BidRepository(IMongoDatabase mongoDatabase):base(mongoDatabase)
        {

        }
        public async Task<AuctionBid> FindBidByAsync(string productId, string buyerId)
        {
            var filter = Builders<AuctionBid>.Filter.Eq(s => s.ProductId, productId);
            filter &= Builders<AuctionBid>.Filter.Eq(s => s.BuyerId, buyerId);

            var result = await this.collection.FindAsync<AuctionBid>(filter);
            return result.FirstOrDefault();
        }

        public async Task<List<AuctionBid>> FindBidByProductIdAsync(string productId)
        {
            var filter = Builders<AuctionBid>.Filter.Eq(s => s.ProductId, productId);

            IAsyncCursor<AuctionBid> result = await this.collection.FindAsync<AuctionBid>(filter);
            return result.ToList();
        }
    }
}
