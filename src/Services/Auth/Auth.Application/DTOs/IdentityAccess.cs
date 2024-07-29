
namespace Auth.Application.DTOs
{
    public class IdentityAccess
    {
        public ApplicationUserDto? User { get; set; }
        public string? AccessToken { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
