using System.ComponentModel.DataAnnotations;

namespace EAuction.Buyer.API.Models
{
    public class AuctionBuyerBidModel
    {
        [Required]
        [MinLength(5)]
        [MaxLength(10)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string LastName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Pin { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string ProductId { get; set; }

        [Required]
        [Range(double.Epsilon, double.MaxValue)]
        public decimal BidAmount { get; set; }
    }
}
