namespace Common.Filters {
    public class ProductFilters {
        public string? Brand { get; set; }
        public string? Category { get; set; }
        public string? SpecialTag { get; set; }
        public string? SearchString { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }
}
