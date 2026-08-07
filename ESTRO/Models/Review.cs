using System.ComponentModel.DataAnnotations;

namespace ESTRO.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string CustomerName { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }
    }
}