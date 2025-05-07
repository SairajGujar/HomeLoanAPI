
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HomeLoanAPI.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HomeLoanAPI.Models
{
    public class LoanApplication
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ValidateNever]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

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

        public DateTime ApplicationDate { get; set; } = DateTime.Now;
        [Required]
        [ValidateNever]
        public string AadharFilePath { get; set; }
        [Required]
        [ValidateNever]
        public string PANFilePath { get; set; }
        [Required]
        [ValidateNever]
        public string SalarySlipPath { get; set; }
        [Required]
        [ValidateNever]
        public string NOCFilePath { get; set; }
        [DefaultValue("Pending")]
        public string? Status { get; set; }
    }
}
