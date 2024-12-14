namespace EnglishSystem.Application.DTOs
{
    public class LessonWithHomeworkDTO
    {
        public int LessonId { get; set; }
        public DateTime LessonDateTime { get; set; }
        public string? Topic { get; set; }
        public string? Description { get; set; }
        public List<HomeworkDTO> Homework { get; set; } = new();
    }
}
