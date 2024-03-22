using Microsoft.AspNetCore.Identity;

namespace Common.Auth {
    public class AppUser : IdentityUser {
        public string Name { get; set; }
    }
}
