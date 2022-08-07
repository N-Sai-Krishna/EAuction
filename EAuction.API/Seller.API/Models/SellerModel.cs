using System.ComponentModel.DataAnnotations;

namespace EAuction.Seller.API.Models
{
    public class SellerModel
    {
        [Required]
        [MinLength(5)]
        [MaxLength(30)]
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
        [MinLength(10)]
        [MaxLength(10)]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
