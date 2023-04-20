namespace Capstone_API.DTO.Task.Fetch
{
    public class CreateExecuteInfoRequest
    {
        public string? ExecuteId { get; set; }
        public DateTime? ExecuteTime { get; set; }
        public int? SemesterId { get; set; }
        public int DepartmentHeadId { get; set; }
    }
}
