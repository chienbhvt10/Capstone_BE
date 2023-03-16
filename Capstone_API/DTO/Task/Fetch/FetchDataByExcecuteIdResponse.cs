

namespace Capstone_API.DTO.Task.Fetch
{
    public class ExecuteData
    {
        public int instructorID { get; set; }
        public int taskID { get; set; }
    }
    public class DataFetch
    {
        public string status { get; set; }
        public ExecuteData[] data { get; set; }
    }
    public class FetchDataByExcecuteIdResponse
    {
        public bool success { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public DataFetch data { get; set; }
    }

    public class FetchExcecuteResponse
    {
        public bool success { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public int data { get; set; }
    }
}
