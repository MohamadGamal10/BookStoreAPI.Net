namespace BooKStore.HTTP
{
    public class Result<T>
    {
        public bool IsSuccess { get; private set; }
        public T? Data { get; private set; }
        public List<string> Errors { get; private set; } = new();
        public string? Message { get; private set; }

        private Result() { }

        public static Result<T> Ok(T data, string? message = null)
            => new()
            {
                IsSuccess = true,
                Data = data,
                Message = message
            };

        public static Result<T> Fail(string error, string? message = null)
            => new()
            {
                IsSuccess = false,
                Errors = new List<string> { error },
                Message = message
            };

        public static Result<T> Fail(List<string> errors, string? message = null)
            => new()
            {
                IsSuccess = false,
                Errors = errors,
                Message = message
            };
    }
}