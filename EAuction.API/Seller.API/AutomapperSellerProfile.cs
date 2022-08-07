using AutoMapper;
using EAuction.Seller.API.Models;
using Seller.Core.Domain;

namespace EAuction.Seller.API
{
    public class AutomapperSellerProfile : Profile
    {
        public AutomapperSellerProfile()
        {
            CreateMap<SellerModel, AuctionProductSeller>();
            CreateMap<ProductModel, AuctionProduct>();
        }
    }
}
