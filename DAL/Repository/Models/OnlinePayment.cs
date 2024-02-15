using Auth;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Repository.Models {
    public class OnlinePayment {
        public int Id { get; set; }
        public int OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }
        public string PaymentId { get; set; }
        public string PaymentStatus { get; set; }
        public double PaymentAmount { get; set; }
        public string? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public AppUser? User { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;

        [NotMapped]
        public string ClientSecret { get; set; }
    }
}
