using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishSystem.Domain.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }

 
        [ForeignKey("EnglishLevelId")]
        public EnglishLevel? EnglishLevel { get; set; }
        public int? EnglishLevelId { get; set; }


        [ForeignKey("GroupId")]
        public Group? Group {  get; set; }
        public int? GroupId { get; set; }

    }
}
