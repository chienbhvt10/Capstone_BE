namespace Capstone_API.Results
{
    public class GenericResult<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }

        public GenericResult()
        {
            IsSuccess = true;
        }

        public GenericResult(T data, bool isSuccess)
        {
            IsSuccess = isSuccess;
            Data = data;
        }

        public GenericResult(string message)
        {
            IsSuccess = false;
            Message = message;
        }
    }
}

