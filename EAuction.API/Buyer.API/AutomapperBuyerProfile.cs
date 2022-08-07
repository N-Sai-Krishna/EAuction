using AutoMapper;
using Buyer.Core.Domain;
using EAuction.Buyer.API.Models;

namespace Buyer.API
{
    public class AutomapperBuyerProfile : Profile
    {
        public AutomapperBuyerProfile()
        {
            CreateMap<AuctionBuyerBidModel, AuctionBuyer>();
            CreateMap<AuctionBuyerBidModel, AuctionBid>();
        }
    }
}
