

namespace Capstone_API.Data.Entities
{
    public class Model
    {
        public int Id { get; set; }
        public int Solver { get; set; }
        public int Strategy { get; set; }
        public int Priority_moving_distance { get; set; }
        public int Input_type { get; set; }
        public int Applied_objectives { get; set; }

        public Model(int id, int solver, int strategy, int priority_moving_distance, int input_type, int applied_objectives)
        {
            Id = id;
            Solver = solver;
            Strategy = strategy;
            Priority_moving_distance = priority_moving_distance;
            Input_type = input_type;
            Applied_objectives = applied_objectives;
        }

        public Model()
        {
        }
    }
}
