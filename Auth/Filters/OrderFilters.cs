namespace Common.Filters {
    public class OrderFilters {
        public string? UserId { get; set; }
        public string? SearchString { get; set; }
        public string? Status { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }
}
