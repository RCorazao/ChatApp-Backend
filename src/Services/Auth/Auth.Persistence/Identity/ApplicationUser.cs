
using Microsoft.AspNetCore.Identity;

namespace Auth.Persistence.Identity
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
