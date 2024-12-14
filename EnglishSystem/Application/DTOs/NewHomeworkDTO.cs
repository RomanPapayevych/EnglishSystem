namespace EnglishSystem.Application.DTOs
{
    public class NewHomeworkDTO
    {
        public int HomeworkId { get; set; }
        public string Content { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
