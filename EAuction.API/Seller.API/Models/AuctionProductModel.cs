using System.ComponentModel.DataAnnotations;

namespace EAuction.Seller.API.Models
{
    public class AuctionProductModel
    {
        [Required]
        public ProductModel Product {get; set; }

        [Required]
        public SellerModel Seller {get; set; }
    }
}
