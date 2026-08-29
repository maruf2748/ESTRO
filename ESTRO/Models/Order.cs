using System.ComponentModel.DataAnnotations;

namespace ESTRO.Models
{
    public class Order
    {
        public int Id { get; set; }

        // GENERATED AUTOMATICALLY
        public string? OrderNumber { get; set; }

        [Required]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        public decimal TotalAmount { get; set; }

        [Required]
        public string PaymentMethod { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending";

        public int ProductId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public string? UserEmail { get; set; }
    }
}
