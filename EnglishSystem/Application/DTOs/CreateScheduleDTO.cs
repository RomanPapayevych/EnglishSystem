namespace EnglishSystem.Application.DTOs
{
    public class CreateScheduleDTO
    {
        public List<DayOfWeek> DaysOfWeek { get; set; } = new List<DayOfWeek>();
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
