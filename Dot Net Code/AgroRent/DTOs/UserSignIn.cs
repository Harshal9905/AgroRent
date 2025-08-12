using System.ComponentModel.DataAnnotations;

namespace AgroRent.DTOs
{
    public class UserSignIn
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
