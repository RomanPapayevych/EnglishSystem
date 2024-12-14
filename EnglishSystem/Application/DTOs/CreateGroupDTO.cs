using EnglishSystem.Domain.Entities;

namespace EnglishSystem.Application.DTOs
{
    public class CreateGroupDTO
    {
        public string Name { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int EnglishLevelId { get; set; }
        public List<DayOfWeek> DaysOfWeek { get; set; } = new List<DayOfWeek>();
    }
}
