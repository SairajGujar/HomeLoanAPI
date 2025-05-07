using System.ComponentModel.DataAnnotations;

namespace HomeLoanAPI.Models
{
    public class LoanApplicationDto
    {
        [Required]
        public string EmploymentType { get; set; }

        [Required]
        public decimal NetMonthlySalary { get; set; }

        public string PropertyName { get; set; }

        public string PropertyLocation { get; set; }

        public decimal EstimatedCost { get; set; }

        [Required]
        public decimal LoanAmountRequested { get; set; }

        public int TenureYears { get; set; }

        [Required]
        public IFormFile AadharFile { get; set; }

        [Required]
        public IFormFile PANFile { get; set; }

        [Required]
        public IFormFile SalarySlipFile { get; set; }

        [Required]
        public IFormFile NOCFile { get; set; }
    }

}
