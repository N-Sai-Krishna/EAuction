using EAuction.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Seller.Core.Domain
{
    internal class AuctionBidProduct : IEntity<string>
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
    }
}
