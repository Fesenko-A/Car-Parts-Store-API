using Microsoft.AspNetCore.Identity;

namespace Auth {
    public class AppUser : IdentityUser {
        public string Name { get; set; }
    }
}
