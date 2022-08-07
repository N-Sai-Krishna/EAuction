using Seller.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Seller.Core.Services
{
    public interface ISellerService
    {
        Task<bool> AddProductAsync(AuctionProduct auctionProduct, AuctionProductSeller auctionProductSeller);

        Task<bool> DeleteProductFromAuctionSync(string productId);
    }
}
