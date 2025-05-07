using HomeLoanAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeLoanAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class CalculatorController : ControllerBase
    {
        [HttpPost("eligibility")]
        public IActionResult CalculateEligibility([FromBody] EligibilityDto dto)
        {
            if (dto.NetMonthlySalary <= 0)
                return BadRequest("Salary must be greater than 0");

            decimal eligibleAmount = 60 * (0.6m * dto.NetMonthlySalary);

            return Ok(new { EligibleLoanAmount = eligibleAmount });
        }

        [HttpPost("emi")]
        public IActionResult CalculateEmi([FromBody] EmiDto dto)
        {
            if (dto.LoanAmount <= 0 || dto.TenureMonths <= 0)
                return BadRequest("Loan amount and tenure must be greater than 0");

            decimal annualRate = 0.085m; // 8.5% annual interest
            decimal monthlyRate = annualRate / 12;
            int n = dto.TenureMonths;
            decimal P = dto.LoanAmount;
            decimal R = monthlyRate;

            decimal emi = P * R * (decimal)(Math.Pow(1 + (double)R, n)) /
                          (decimal)(Math.Pow(1 + (double)R, n) - 1);

            return Ok(new { EMI = Math.Round(emi, 2) });
        }
    }
}
