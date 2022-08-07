using BidListing.Core.Domain;
using EAuction.Persistence;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace BidListing.Core.Repositories
{
    internal class ProductAndBidDetailsRepository : Repository<ProductAndBidDetails, string>
    {
        public ProductAndBidDetailsRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {

        }

    }
}
