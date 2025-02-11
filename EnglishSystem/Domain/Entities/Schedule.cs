using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EnglishSystem.Domain.Entities
{
    public class Schedule
    {
        [Key]
        public int Id { get; set; }
        public List<DayOfWeek>? DaysOfWeek { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        [ForeignKey("GroupId")]
        public Group Group { get; set; } = null!;
        public int GroupId { get; set; }

        [JsonIgnore]
        public ICollection<Lesson>? Lessons { get; set; }
    }
}
