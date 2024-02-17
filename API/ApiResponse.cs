using System.Net;

namespace API {
    public class ApiResponse {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public bool IsSuccess { get; set; } = true;
        public object? Result { get; set; } = null;
        public string? ErrorMessage { get; set; } = string.Empty;

        public ApiResponse(HttpStatusCode statusCode, bool isSuccess, object? result, string? errorMessage) {
            StatusCode = statusCode; 
            IsSuccess = isSuccess; 
            Result = result; 
            ErrorMessage = errorMessage;
        }

        public ApiResponse(HttpStatusCode statusCode, bool isSuccess, string? errorMessage) {
            StatusCode = statusCode;
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public ApiResponse(object successfulResult) {
            Result = successfulResult;
        }

        public ApiResponse(string errorMessage, bool isSuccess) {
            ErrorMessage = errorMessage;
            IsSuccess = isSuccess;
        }
    }
}
