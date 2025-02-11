namespace EnglishSystem.Application.DTOs
{
    public class NormalLessonWithHomeworkDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? Topic { get; set; }
        public string? Description { get; set; }
        public List<HomeworkDTO> Homework { get; set; } = new();
    }
}
