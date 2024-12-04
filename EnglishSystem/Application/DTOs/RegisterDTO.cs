using System.ComponentModel.DataAnnotations;

namespace EnglishSystem.Application.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "First name can't be blank")]
        public string? FirstName { get; set; }


        [Required(ErrorMessage = "Last name can't be blank")]
        public string? LastName { get; set; }


        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage = "Email should be in a proper email address format")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        

        [Required(ErrorMessage = "Phone number can't be blank")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\+380|0)[0-9]{9}$", ErrorMessage = "Phone number must be in the Ukrainian format (+380XXXXXXXXX or 0XXXXXXXXX)")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password can't be blank")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirm Password can't be blank")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirm password do not match")]
        public string? ConfirmPassword { get; set; }
    }
}
