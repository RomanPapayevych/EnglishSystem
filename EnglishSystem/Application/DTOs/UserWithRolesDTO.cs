namespace EnglishSystem.Application.DTOs
{
    public class UserWithRolesDTO
    {
        public int? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public IList<string>? Roles { get; set; }
    }
}
