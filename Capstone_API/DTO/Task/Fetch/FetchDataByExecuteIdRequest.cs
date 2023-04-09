namespace Capstone_API.DTO.Task.Fetch
{
    public class FetchDataByExecuteIdRequest
    {
        public string? token { get; set; }
        public string? sessionHash { get; set; }
        public int solutionNo { get; set; }
    }
}
