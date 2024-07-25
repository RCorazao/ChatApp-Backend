using System.ComponentModel.DataAnnotations;

namespace Auth.Application.DTOs
{
    public class SignInRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
