using Seller.Core.Validators;
using System;
using System.ComponentModel.DataAnnotations;

namespace EAuction.Seller.API.Models
{
    public class ProductModel
    {

        [Required]
        [MinLength(5)]
        [MaxLength(30)]
        public string ProductName { get; set; }

        [Required]
        public string ShortDescription { get; set; }

        [Required]
        [IsValidCategory]
        public string Category { get; set; }

        [Required]
        [Range(double.Epsilon, double.MaxValue)]
        public decimal StartingPrice { get; set; }

        [Required]
        public DateTime BidEndDate { get; set; }
    }
}
