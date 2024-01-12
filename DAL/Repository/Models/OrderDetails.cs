using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Repository.Models {
    public class OrderDetails {
        [Key]
        public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public double Price { get; set; }
    }
}