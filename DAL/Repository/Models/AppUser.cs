using Microsoft.AspNetCore.Identity;

namespace DAL.Repository.Models {
    public class AppUser : IdentityUser {
        public string Name { get; set; }
    }
}
