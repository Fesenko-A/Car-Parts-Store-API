using Common.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Repository.Models
{
    public class Order {
        [Key]
        public int OrderId { get; set; }
        [Required]
        public string PickupName { get; set; }
        [Required]
        public string PickupPhoneNumber { get; set; }
        [Required]
        public string PickupEmail { get; set; }

        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public AppUser User { get; set; }
        public double OrderTotal { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public int TotalItems { get; set; }
        public int? PaymentMethodId { get; set; }
        [ForeignKey(nameof(PaymentMethodId))]
        public PaymentMethod PaymentMethod { get; set; }
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
        public bool? Paid { get; set; } = false;

        public IEnumerable<OrderDetails> OrderDetails { get; set; }
    }
}
