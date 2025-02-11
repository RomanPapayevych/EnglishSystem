using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EnglishSystem.Domain.Entities
{
    public class Group
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        //[JsonIgnore]
        public Schedule Schedule { get; set; } = null!;
        [ForeignKey("EnglishLevelId")]
        public EnglishLevel EnglishLevel { get; set; } = null!;
        public int EnglishLevelId { get; set; }

        [ForeignKey(nameof(TeacherId))]
        public ApplicationUser? Teacher { get; set; }
        public int? TeacherId { get; set; }

        [JsonIgnore]
        public ICollection<ApplicationUser>? Students { get; set; }
    }
}
