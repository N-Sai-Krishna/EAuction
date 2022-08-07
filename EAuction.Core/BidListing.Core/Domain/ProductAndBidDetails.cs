using EAuction.Persistence;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BidListing.Core.Domain
{
    public class ProductAndBidDetails : IEntity<string>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set ; }

        public string ProductName { get; set; }
        public string ShortDescription { get; set; }
        public string Category { get; set; }
        public decimal StartingPrice { get; set; }
        public DateTime BidEndDate { get; set; }

        public IList<BidDetails> Bids { get; set; }

    }
}
