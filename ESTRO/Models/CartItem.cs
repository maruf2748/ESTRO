using System.ComponentModel.DataAnnotations;

namespace ESTRO.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public string? UserEmail { get; set; }

        public int ProductId { get; set; }

        public Product? Product { get; set; }

        public int Quantity { get; set; }
    }
}