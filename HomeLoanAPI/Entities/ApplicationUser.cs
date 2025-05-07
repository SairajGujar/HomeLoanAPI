using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HomeLoanAPI.Entities
{
    public class ApplicationUser : IdentityUser
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
    }
}
