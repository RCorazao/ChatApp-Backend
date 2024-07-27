
using Microsoft.AspNetCore.Identity;

namespace Auth.Persistence.Identity
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? Avatar { get; set; }
    }
}
