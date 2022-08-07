using System;
using System.Collections.Generic;
using System.Text;

namespace BidListing.Core.Domain.Messages
{
    public class AuctionProduct
    {
        public string Id { get; set; }

        public string ProductName { get; set; }
        public string ShortDescription { get; set; }
        public string Category { get; set; }
        public decimal StartingPrice { get; set; }
        public DateTime BidEndDate { get; set; }

        public string SellerId { get; set; }
    }
}
