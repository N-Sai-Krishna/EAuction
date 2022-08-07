using EAuction.Persistence;
using MongoDB.Driver;
using Seller.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Seller.Core.Repositories
{
    internal class SellerRepository : Repository<AuctionProductSeller, string>
    {
        public SellerRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {

        }

        public override async Task<AuctionProductSeller> FindByAsync(string key)
        {
            var filter = Builders<AuctionProductSeller>.Filter.Eq(s=>s.Phone, key);

            var result = await this.collection.FindAsync<AuctionProductSeller>(filter);

            return result.FirstOrDefault();
        }
    }
}
