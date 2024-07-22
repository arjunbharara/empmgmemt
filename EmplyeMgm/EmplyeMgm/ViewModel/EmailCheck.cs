using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace EmplyeMgm.ViewModel
{
    public class EmailCheckAttribute : ValidationAttribute
    {
        private string error;
        public override string FormatErrorMessage(string name)
        {
           return error;
        }
        public override bool IsValid(object? value)
        {
            var email = value as string;

            if (string.IsNullOrWhiteSpace(email))
            {

                error= "Email cannot be empty.";
                return false;
            }

            // Check if the email is well-formed
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!emailRegex.IsMatch(email))
            {

                error = "Invalid email format.";
                return false;
            }
            return true;
        }
    }
}
