using EnglishSystem.Domain.Entities;
using Microsoft.VisualBasic;

namespace EnglishSystem.Application.DTOs
{
    public class CreateGroupDTO
    {
        public string Name { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int EnglishLevelId { get; set; }
        public List<DayOfWeek>? DaysOfWeek { get; set; }
        public DateTime StartTimeOfLesson { get; set; }
        public DateTime EndTimeOfLesson { get; set; }
    }
}
