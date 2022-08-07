using System;
using System.Collections.Generic;
using System.Text;

namespace Buyer.Core.Domain.Messages
{
    public class AuctionBuyerBidDetails
    {
        public string ProductId { get; set; }
        public string FirstName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public decimal BidAmount { get; set; }
    }
}
