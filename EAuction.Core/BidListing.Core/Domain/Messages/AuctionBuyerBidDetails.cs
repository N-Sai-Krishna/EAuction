using System;
using System.Collections.Generic;
using System.Text;

namespace BidListing.Core.Domain.Messages
{
    public class AuctionBuyerBidDetails
    {
        public string FirstName { get; set; }
        public string Phone { get; set; }
        public decimal BidAmount { get; set; }
        public string Email { get; set; }
    }
}
