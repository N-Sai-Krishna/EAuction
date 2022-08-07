using EAuction.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Buyer.Core.Domain.Messages
{
    internal class AuctionProduct : IEntity<string>
    {
        public string Id { get; set ; }
    }
}
