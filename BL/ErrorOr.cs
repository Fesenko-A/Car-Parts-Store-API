namespace BL {
    public class ErrorOr<T> {
        public T? Value { get; set; }
        public string? Message { get; set; }

        public ErrorOr(T value) {
            Value = value;
        }

        public ErrorOr(string message) {
            Message = message;
        }

        public ErrorOr(T? value, string message) {
            Value = value;
            Message = message;
        }
    }
}
