namespace API.Utility {
    public class Pagination {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }

        public Pagination(int currentPage, int pageSize, int totalRecords) { 
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalRecords = totalRecords;
        }
    }
}
