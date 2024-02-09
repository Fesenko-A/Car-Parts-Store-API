using System.ComponentModel.DataAnnotations;

namespace DAL.Repository.Models {
    public class PaymentMethod {
        [Key]
        public int PaymentMethodId { get; set; }
        public string Description { get; set; }
    }
}
