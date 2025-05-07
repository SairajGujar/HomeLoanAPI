using HomeLoanAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using HomeLoanAPI.Data;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles ="User")]
[Route("api/[controller]")]
[ApiController]
public class LoanController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;

    public LoanController(ApplicationDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    [HttpPost("apply")]
    public async Task<IActionResult> ApplyLoan([FromForm] LoanApplicationDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = User.FindFirstValue(ClaimTypes.Name);
        if (userId == null)
            return Unauthorized();

        // Upload files and get paths
        var uploadFolder = Path.Combine(_env.WebRootPath, "uploads");
        Directory.CreateDirectory(uploadFolder); // Ensure it exists

        string SaveFile(IFormFile file)
        {
            var uniqueName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var path = Path.Combine(uploadFolder, uniqueName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return $"/uploads/{uniqueName}";
        }

        var application = new LoanApplication
        {
            UserId = userId,
            EmploymentType = dto.EmploymentType,
            NetMonthlySalary = dto.NetMonthlySalary,
            PropertyName = dto.PropertyName,
            PropertyLocation = dto.PropertyLocation,
            EstimatedCost = dto.EstimatedCost,
            LoanAmountRequested = dto.LoanAmountRequested,
            TenureYears = dto.TenureYears,
            AadharFilePath = SaveFile(dto.AadharFile),
            PANFilePath = SaveFile(dto.PANFile),
            SalarySlipPath = SaveFile(dto.SalarySlipFile),
            NOCFilePath = SaveFile(dto.NOCFile),
            ApplicationDate = DateTime.Now,
            Status = "Pending"
        };

        _context.LoanApplications.Add(application);
        await _context.SaveChangesAsync();

        return Ok(new { application.Id, Message = "Loan application submitted with documents." });
    }
    [HttpGet("my-applications")]
    [Authorize(Roles ="User")]
    public async Task<IActionResult> GetMyApplications()
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);

        var applications = await _context.LoanApplications
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.ApplicationDate)
            .Select(a => new
            {
                a.Id,
                a.Status,
                a.LoanAmountRequested,
                a.TenureYears,
                a.ApplicationDate
            })
            .ToListAsync();

        return Ok(applications);
    }

}
