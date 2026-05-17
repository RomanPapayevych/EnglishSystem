using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace EnglishSystem.Domain.Entities
{
    public class EnglishLevel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Level { get; set; } = null!;
        public string? Description { get; set; } 
        public string? ImageUrl { get; set; } 
        public ICollection<Group>? Groups { get; set; }

    }
}
