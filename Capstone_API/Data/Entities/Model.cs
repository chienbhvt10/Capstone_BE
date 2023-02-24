

namespace Capstone_API.Data.Entities
{
    public class Model
    {
        public int Id { get; set; }
        public string? Solver { get; set; }
        public string? Strategy { get; set; }
        public string? InputType { get; set; }
        public int PriorityMovingDistanceSettingLevel { get; set; }
        public int MinimizeCostOfTimeSettingLevel { get; set; }
        public int MinimizeNumberOfSubjectsSettingLevel { get; set; }
        public int QuotaOfClassSettingLevel { get; set; }
        public int PreferenceLevelOfSubjectSettingLevel { get; set; }
        public int PreferenceLevelOfSlotSettingLevel { get; set; }

        public Model(int id, string solver, string strategy, string input_type, int priorityMovingDistanceSettingLevel, int minimizeCostOfTimeSettingLevel, int minimizeNumberOfSubjectsSettingLevel, int quotaOfClassSettingLevel, int preferenceLevelOfSubjectSettingLevel, int preferenceLevelOfSlotSettingLevel)
        {
            Id = id;
            Solver = solver;
            Strategy = strategy;
            InputType = input_type;
            PriorityMovingDistanceSettingLevel = priorityMovingDistanceSettingLevel;
            MinimizeCostOfTimeSettingLevel = minimizeCostOfTimeSettingLevel;
            MinimizeNumberOfSubjectsSettingLevel = minimizeNumberOfSubjectsSettingLevel;
            QuotaOfClassSettingLevel = quotaOfClassSettingLevel;
            PreferenceLevelOfSubjectSettingLevel = preferenceLevelOfSubjectSettingLevel;
            PreferenceLevelOfSlotSettingLevel = preferenceLevelOfSlotSettingLevel;
        }

        public Model()
        {
        }
    }
}
