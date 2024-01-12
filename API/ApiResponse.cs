using System.Net;

namespace API {
    public class ApiResponse {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public bool IsSuccess { get; set; } = true;
        public object? Result { get; set; }
    }
}
