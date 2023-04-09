

namespace Capstone_API.DTO.Task.Fetch
{
    public class ExecuteData
    {
        public string? instructorId { get; set; }
        public string? taskId { get; set; }
        public string? slotId { get; set; }
    }

    public class DataFetch
    {
        public int status { get; set; }
        public int numberofsolution { get; set; }
        public int taskAssigned { get; set; }
        public int workingDay { get; set; }
        public int workingTime { get; set; }
        public int waitingTime { get; set; }
        public int subjectDiversity { get; set; }
        public int quotaAvailable { get; set; }
        public int walkingDistance { get; set; }
        public int subjectPreference { get; set; }
        public int slotPreference { get; set; }
        public List<ExecuteData>? results { get; set; }
    }
    public class FetchDataByExcecuteIdResponse
    {
        public bool success { get; set; }
        public int code { get; set; }
        public string? message { get; set; }
        public DataFetch? data { get; set; }
    }

    public class FetchExcecuteResponse
    {
        public bool success { get; set; }
        public int code { get; set; }
        public string? message { get; set; }
        public int data { get; set; }
    }
}
