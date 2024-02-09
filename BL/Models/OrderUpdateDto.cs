namespace BL.Models {
    public class OrderUpdateDto {
        public int OrderId { get; set; }
        public string? PickupName { get; set; }
        public string? PickupPhoneNumber { get; set; }
        public string? PickupEmail { get; set; }

        public string? Status { get; set; }
        public DateTime? LastUpdate { get; set; } = DateTime.UtcNow;
    }
}
