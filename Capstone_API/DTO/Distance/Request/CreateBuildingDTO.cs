namespace Capstone_API.DTO.Distance.Request
{
    public class CreateBuildingDTO
    {
        public string? Name { get; set; }
        public string? ShortName { get; set; }
        public int DepartmentHeadId { get; set; }
        public int SemesterId { get; set; }
    }
}
