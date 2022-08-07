using EAuction.Persistence;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Seller.Core.Domain
{
    public class AuctionProduct : IEntity<string>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string ProductName { get; set; }
        public string ShortDescription { get; set; }
        public string Category { get; set; }
        public decimal StartingPrice { get; set; }
        public DateTime BidEndDate { get; set; }
        public string SellerId { get; set; }
    }
}
