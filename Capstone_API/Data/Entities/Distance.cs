namespace Capstone_API.Data.Entities
{
    public class Distance
    {

        public int Id { get; set; }
        public int Building_1_id { get; set; }
        public int Building_2_id
        {
            get; set;
        }
        public int DistanceBetween
        {
            get; set;
        }
        public Distance(int id, int building_1_id, int building_2_id, int distanceBetween)
        {
            Id = id;
            Building_1_id = building_1_id;
            Building_2_id = building_2_id;
            DistanceBetween = distanceBetween;
        }

        public Distance()
        {
        }
    }
}
