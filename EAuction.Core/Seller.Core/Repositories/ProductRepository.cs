using EAuction.Persistence;
using MongoDB.Driver;
using Seller.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Seller.Core.Repositories
{
    internal class ProductRepository : Repository<AuctionProduct, string>
    {
        public ProductRepository(IMongoDatabase mongoDatabase) :base(mongoDatabase)
        {

        }
    }
}
