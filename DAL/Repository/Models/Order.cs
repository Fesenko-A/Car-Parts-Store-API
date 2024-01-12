using Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Repository.Models {
    public class Order {
        [Key]
        public int Id { get; set; }
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
        public string PaymentId { get; set; }
        public string Status { get; set; }
        public int TotalItems { get; set; }

        public IEnumerable<OrderDetails> OrderDetails { get; set; }
    }
}
