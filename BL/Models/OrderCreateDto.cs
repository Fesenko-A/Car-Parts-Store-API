namespace BL.Models {
    public class OrderCreateDto {
        public string PickupName { get; set; }
        public string PickupPhoneNumber { get; set; }
        public string PickupEmail { get; set; }
        public string UserId { get; set; }
        public double OrderTotal { get; set; }
        public string Status { get; set; }
        public int TotalItems { get; set; }
        public int PaymentMethodId { get; set; }

        public IEnumerable<OrderDetailsCreateDto> OrderDetails { get; set; }
    }
}
