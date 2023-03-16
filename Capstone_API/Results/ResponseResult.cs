namespace Capstone_API.Results
{
    public class ResponseResult
    {
        public string Message { get; set; } = string.Empty;

        public bool IsSuccess { get; set; }

        public ResponseResult()
        {
            IsSuccess = true;
        }

        public ResponseResult(string message)
        {
            IsSuccess = false;
            Message = message;
        }
        public ResponseResult(string message, bool isSuccess)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
    }
}