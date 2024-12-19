namespace EnglishSystem.Application.DTOs
{
    public class GroupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int EnglishLevelId { get; set; }
        public string? EnglishLevel { get; set; }
        public object? Teacher { get; set; } 
        public List<string> DaysOfWeek { get; set; } = new();
    }
}
