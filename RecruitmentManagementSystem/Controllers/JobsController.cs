using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentManagementSystem.Data;
using RecruitmentManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace RecruitmentManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class JobsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public JobsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/jobs - All authenticated users can access
        [HttpGet]
        public async Task<IActionResult> GetJobs()
        {
            var jobs = await _context.Jobs
                .Include(j => j.PostedBy)
                .Select(j => new
                {
                    j.Id,
                    j.Title,
                    j.Description,
                    j.CompanyName,
                    j.PostedOn,
                    j.TotalApplications,
                    PostedBy = j.PostedBy.Name
                })
                .ToListAsync();

            return Ok(jobs);
        }

        // GET: api/jobs/apply?job_id={job_id} - Only Applicant users
        [HttpGet("apply")]
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> ApplyForJob([FromQuery] int job_id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            // Check if job exists
            var job = await _context.Jobs.FindAsync(job_id);
            if (job == null)
                return NotFound("Job not found");

            // Check if already applied
            var existingApplication = await _context.JobApplications
                .FirstOrDefaultAsync(ja => ja.JobId == job_id && ja.ApplicantId == userId);

            if (existingApplication != null)
                return BadRequest("You have already applied for this job");

            // Create application
            var application = new JobApplication
            {
                JobId = job_id,
                ApplicantId = userId,
                AppliedOn = DateTime.UtcNow
            };

            _context.JobApplications.Add(application);

            // Update total applications count
            job.TotalApplications++;

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Application submitted successfully" });
        }
    }
}