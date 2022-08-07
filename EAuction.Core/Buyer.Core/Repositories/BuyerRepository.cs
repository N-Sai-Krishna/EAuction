using Buyer.Core.Domain;
using EAuction.Persistence;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Buyer.Core.Repositories
{
    internal class BuyerRepository : Repository<AuctionBuyer, string>
    {
        public BuyerRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {

        }

        public override async Task<AuctionBuyer> FindByAsync(string key)
        {
            var filter = Builders<AuctionBuyer>.Filter.Eq(s => s.Phone, key);

            var result = await this.collection.FindAsync<AuctionBuyer>(filter);
            return result.FirstOrDefault();
        }
    }
}
