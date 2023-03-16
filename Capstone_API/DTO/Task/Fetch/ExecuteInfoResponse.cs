namespace Capstone_API.DTO.Task.Fetch
{
    public class ExecuteInfoResponse
    {
        public int Id { get; set; }
        public int? ExecuteId { get; set; }
        public DateTime? ExecuteTime { get; set; }
        public int? SemesterId { get; set; }
    }
}
