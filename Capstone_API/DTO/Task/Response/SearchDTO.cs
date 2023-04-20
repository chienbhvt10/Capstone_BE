namespace Capstone_API.DTO.Task.Response
{
    public class SearchDTO
    {
        public List<ResponseTaskByLecturerIsKey>? DataAssign { get; set; }
        public TimeSlotInfoResponse? DataNotAssign { get; set; }
    }
}
