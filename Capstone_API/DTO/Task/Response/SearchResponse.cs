namespace Capstone_API.DTO.Task.Response
{
    public class SearchResponse
    {
        public List<ResponseTaskByLecturerIsKey>? DataAssign { get; set; }
        public TimeSlotInfoResponse? DataNotAssign { get; set; }
    }
}
