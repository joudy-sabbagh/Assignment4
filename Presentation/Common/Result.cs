// Presentation/Common/Result.cs
namespace Presentation.Common
{
    public class Result<T>
    {
        public bool Success { get; }
        public string? Message { get; }
        public T? Data { get; }

        private Result(bool success, T? data, string? message)
        {
            Success = success;
            Data = data;
            Message = message;
        }

        public static Result<T> SuccessResult(T data, string? message = null) =>
            new Result<T>(true, data, message);

        public static Result<T> FailureResult(string message) =>
            new Result<T>(false, default, message);
    }
}
