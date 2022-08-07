using System;
using System.Collections.Generic;
using System.Text;

namespace BidListing.Core.Domain.Messages
{
    public class BidAddOrUpdateMessage
    {
        public string ProductId { get; set; }

        public IList<AuctionBuyerBidDetails> AuctionBuyerBidDetails { get; set; }
    }

}
