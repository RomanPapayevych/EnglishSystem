using Microsoft.AspNetCore.Identity;

namespace EnglishSystem.Domain.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
