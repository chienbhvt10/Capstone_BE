namespace UTA.T2.MusicLibrary.Service.Results
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
    }
}