using Microsoft.AspNetCore.Identity;

namespace EmplyeMgm.Models
{
    public class ApplicationUser :IdentityUser
    {
        public string FirstName;
        public string LastName;
    }
}
