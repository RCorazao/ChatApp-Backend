
using System.ComponentModel.DataAnnotations;

namespace Auth.Application.DTOs
{
    public class SignUpRequest
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
