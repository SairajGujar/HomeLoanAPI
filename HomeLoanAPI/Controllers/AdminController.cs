using System;
using HomeLoanAPI.Data;
using HomeLoanAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeLoanAPI.Controllers
{
    [Authorize(Roles ="Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public AdminController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }


        // GET: api/admin/all-loans
        [HttpGet("all-loans")]
        public async Task<IActionResult> GetAllLoans()
        {
            var loans = await _context.LoanApplications
                .Include(l => l.User)
                .OrderByDescending(l => l.ApplicationDate)
                .ToListAsync();

            return Ok(loans);
        }

        // GET: api/admin/loan/{id}
        [HttpGet("loan/{id}")]
        public async Task<IActionResult> GetLoanDetails(int id)
        {
            var loan = await _context.LoanApplications
                .Include(l => l.User)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (loan == null)
                return NotFound("Loan application not found.");

            return Ok(loan);
        }

        // PUT: api/admin/loan/approve/{id}
        [HttpPut("loan/approve/{id}")]
        public async Task<IActionResult> ApproveLoan(int id)
        {
            var loan = await _context.LoanApplications.FindAsync(id);
            if (loan == null) return NotFound("Loan not found");
            if (loan.Status == "Approved" || loan.Status == "Rejected")
                return BadRequest("Loan decision is already finalized.");


            var user = await _context.Users.FindAsync(loan.UserId);
            if (user == null) return NotFound("User not found");

            loan.Status = "Approved";
            await _context.SaveChangesAsync();

            await _emailService.SendEmailAsync(user.Email,
                "Loan Approved - Home Loan Portal",
                $"Dear {user.FullName},<br><br>Your loan application (ID: {loan.Id}) has been <strong>approved</strong>.<br>Thank you for using our service.<br><br>Regards,<br>Home Loan Portal");

            return Ok("Loan approved and email sent.");
        }

        [HttpPut("loan/reject/{id}")]
        public async Task<IActionResult> RejectLoan(int id)
        {
            var loan = await _context.LoanApplications.FindAsync(id);
            if (loan == null) return NotFound("Loan not found");
            if (loan.Status == "Approved" || loan.Status == "Rejected")
                return BadRequest("Loan decision is already finalized.");


            var user = await _context.Users.FindAsync(loan.UserId);
            if (user == null) return NotFound("User not found");

            loan.Status = "Rejected";
            await _context.SaveChangesAsync();

            await _emailService.SendEmailAsync(user.Email,
                "Loan Rejected - Home Loan Portal",
                $"Dear {user.FullName},<br><br>We regret to inform you that your loan application (ID: {loan.Id}) has been <strong>rejected</strong>.<br>Please review eligibility criteria and try again.<br><br>Regards,<br>Home Loan Portal");

            return Ok("Loan rejected and email sent.");
        }


        // GET: api/admin/loans-by-status?status=Pending
        [HttpGet("loans-by-status")]
        public async Task<IActionResult> GetLoansByStatus([FromQuery] string status)
        {
            var loans = await _context.LoanApplications
                .Include(l => l.User)
                .Where(l => l.Status == status)
                .OrderByDescending(l => l.ApplicationDate)
                .ToListAsync();

            return Ok(loans);
        }
    }
}
