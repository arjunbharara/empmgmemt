using System.ComponentModel.DataAnnotations;

namespace EmplyeMgm.ViewModel
{
    public class EmployeeViewModel
    { 

            public int Id { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "First name must be between 2 and 50 characters")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Last name must be between 2 and 50 characters")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        public string Emial { get; set; }
        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        public DateOnly DOB { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

            public bool IsAdmin { get; set; }

            // Password fields with validation
            [Required(ErrorMessage = "Password is required")]
            [DataType(DataType.Password)]
            [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
            public string Password { get; set; }

            [Required(ErrorMessage = "Confirm Password is required")]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]
            public string ConfirmPassword { get; set; }
    }
}
