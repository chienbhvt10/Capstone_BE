namespace Capstone_API.Results
{
    public class GenericResult<T>
    {
        public GenericResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }

    public class GenericResult : GenericResult<object>
    {
        public GenericResult(bool success, string message) : base(success, message)
        {

        }
    }
}
