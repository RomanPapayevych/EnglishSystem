using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EnglishSystem.Domain.Entities
{
    public class Lesson
    {
        [Key]
        public int Id { get; set; }
        public DateTime LessonDateTime { get; set; }
        public string? Topic { get; set; }
        public string? Description { get; set; }
        [ForeignKey("ScheduleId")]
        public Schedule Schedule { get; set; } = null!;
        public int ScheduleId { get; set; }

        [JsonIgnore]
        public ICollection<Homework> HomeworkAssignments { get; set; } = new List<Homework>();
    }
}
