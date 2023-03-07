using System;
using System.Collections.Generic;

namespace Capstone_API.Models
{
    public partial class SettingModel
    {
        public int Id { get; set; }
        public string? Solver { get; set; }
        public string? Strategy { get; set; }
        public string? InputType { get; set; }
        public int? PriorityMovingDistanceSettingLevel { get; set; }
        public int? MinimizeCostOfTimeSettingLevel { get; set; }
        public int? MinimizeNumberOfSubjectsSettingLevel { get; set; }
        public int? QuotaOfClassSettingLevel { get; set; }
        public int? PreferenceLevelOfSubjectSettingLevel { get; set; }
        public int? PreferenceLevelOfSlotSettingLevel { get; set; }
        public DateTime? CreateOn { get; set; }
        public DateTime? UpdateOn { get; set; }
        public string? ExistStatus { get; set; }
    }
}
