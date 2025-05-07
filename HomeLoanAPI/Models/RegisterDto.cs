using System.ComponentModel.DataAnnotations;

namespace HomeLoanAPI.Models
{
    public class RegisterDto
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "Aadhar number must be 12 digits.")]
        public string AadharNo { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "PAN number must be 10 characters.")]
        public string PANNo { get; set; }

        [Required]
        public DateOnly DOB { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        [Phone]
        public string Phone {  get; set; }
        [Required]
        public string Role { get; set; } // e.g., "User" or "Admin"
    }
}
