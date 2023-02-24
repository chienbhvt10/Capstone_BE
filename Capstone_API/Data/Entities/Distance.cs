namespace Capstone_API.Data.Entities
{
    public class Distance
    {

        public int Id { get; set; }
        public int Building1Id { get; set; }
        public int Building2Id { get; set; }
        public int DistanceBetween { get; set; }
        public Distance(int id, int building_1_id, int building_2_id, int distanceBetween)
        {
            Id = id;
            Building1Id = building_1_id;
            Building2Id = building_2_id;
            DistanceBetween = distanceBetween;
        }

        public Distance()
        {
        }
    }
}
