using AutoMapper;
using BidListing.API.Models;
using BidListing.Core.Domain;

namespace EAuction.Buyer.API
{
    public class AutoMapperBidListingProfile :Profile
    {
        public AutoMapperBidListingProfile()
        {
            CreateMap<ProductAndBidDetails, ProductBidDetailsModel>();
            CreateMap<BidDetails, BidDetailsModel>();
        }

    }
}
