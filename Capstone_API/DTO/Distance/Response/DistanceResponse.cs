namespace Capstone_API.DTO.Distance.Response
{
    public class DistanceResponse
    {
        public int BuildingId { get; set; }
        public string? BuildingName { get; set; }
        public int SemesterId { get; set; }
        public List<BuildingDistance>? BuildingDistances { get; set; }
    }
    public class BuildingDistance
    {
        public int Id { get; set; }
        public int BuildingDistanceId { get; set; }
        public int DistanceBetween { get; set; }
    }
}
