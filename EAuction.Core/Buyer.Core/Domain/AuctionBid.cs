using EAuction.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Buyer.Core.Domain
{
    public class AuctionBid : IEntity<string>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string ProductId { get; set; }
        public string BuyerId { get; set; }
        
        public decimal BidAmount { get; set; }

    }
}
