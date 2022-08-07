using System;
using System.Collections.Generic;

namespace BidListing.API.Models
{
    public class ProductBidDetailsModel
    {

        public string Id { get; set; }
        public string ProductName { get; set; }
        public string ShortDescription { get; set; }
        public string Category { get; set; }
        public decimal StartingPrice { get; set; }
        public DateTime BidEndDate { get; set; }
        public IList<BidDetailsModel> Bids { get; set; }
    }
}
