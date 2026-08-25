using System.ComponentModel.DataAnnotations;

namespace ESTRO.Models
{
    public class WishlistItem
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }
    }
}