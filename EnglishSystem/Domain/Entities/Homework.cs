using EnglishSystem.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishSystem.Domain.Entities
{
    public class Homework
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public HomeworkStatus Status { get; set; } = HomeworkStatus.Active;

        [ForeignKey("LessonId")]
        public Lesson Lesson { get; set; } = null!;
        public int LessonId { get; set; }
    }
}
