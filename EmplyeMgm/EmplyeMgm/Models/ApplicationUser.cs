using Microsoft.AspNetCore.Identity;

namespace EmplyeMgm.Models
{
    public class ApplicationUser :IdentityUser
    {
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DOB {  get; set; }
        public string City  { get; set; }

        public bool IsAdmin{ get; set; }

    }
}
