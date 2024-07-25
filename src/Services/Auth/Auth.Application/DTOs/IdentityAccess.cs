
namespace Auth.Application.DTOs
{
    public class IdentityAccess
    {
        public bool Succeeded { get; set; }
        public string AccessToken { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
