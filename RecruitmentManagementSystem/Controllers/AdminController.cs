using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentManagementSystem.Data;
using RecruitmentManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace RecruitmentManagementSystem.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]  // Only Admin users
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/admin/job - Create job opening
        [HttpPost("job")]
        public async Task<IActionResult> CreateJob([FromBody] CreateJobDto jobDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var job = new Job
            {
                Title = jobDto.Title,
                Description = jobDto.Description,
                CompanyName = jobDto.CompanyName,
                PostedById = userId,
                PostedOn = DateTime.UtcNow,
                TotalApplications = 0
            };

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Job created successfully", JobId = job.Id });
        }

        // GET: api/admin/job/{job_id} - Get job details with applicants
        [HttpGet("job/{job_id}")]
        public async Task<IActionResult> GetJobWithApplicants(int job_id)
        {
            var job = await _context.Jobs
                .Include(j => j.PostedBy)
                .Include(j => j.Applications)
                    .ThenInclude(a => a.Applicant)
                .FirstOrDefaultAsync(j => j.Id == job_id);

            if (job == null)
                return NotFound("Job not found");

            var result = new
            {
                job.Id,
                job.Title,
                job.Description,
                job.CompanyName,
                job.PostedOn,
                job.TotalApplications,
                PostedBy = job.PostedBy.Name,
                Applicants = job.Applications.Select(a => new
                {
                    ApplicantId = a.Applicant.Id,
                    ApplicantName = a.Applicant.Name,
                    ApplicantEmail = a.Applicant.Email,
                    AppliedOn = a.AppliedOn
                }).ToList()
            };

            return Ok(result);
        }

        // GET: api/admin/applicants - Get all applicants
        [HttpGet("applicants")]
        public async Task<IActionResult> GetAllApplicants()
        {
            var applicants = await _context.Users
                .Where(u => u.UserType == UserType.Applicant)
                .Include(u => u.Profile)
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.Email,
                    u.Address,
                    u.ProfileHeadline,
                    Profile = u.Profile == null ? null : new
                    {
                        u.Profile.Skills,
                        u.Profile.Education,
                        u.Profile.Experience,
                        u.Profile.Phone,
                        HasResume = !string.IsNullOrEmpty(u.Profile.ResumeFileAddress)
                    }
                })
                .ToListAsync();

            return Ok(applicants);
        }

        // GET: api/admin/applicant/{applicant_id} - Get applicant details with extracted data
        [HttpGet("applicant/{applicant_id}")]
        public async Task<IActionResult> GetApplicantDetails(int applicant_id)
        {
            var applicant = await _context.Users
                .Where(u => u.Id == applicant_id && u.UserType == UserType.Applicant)
                .Include(u => u.Profile)
                .Include(u => u.Applications)
                    .ThenInclude(a => a.Job)
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.Email,
                    u.Address,
                    u.ProfileHeadline,
                    Profile = u.Profile == null ? null : new
                    {
                        u.Profile.Skills,
                        u.Profile.Education,
                        u.Profile.Experience,
                        u.Profile.Phone,
                        u.Profile.ResumeFileAddress
                    },
                    JobApplications = u.Applications.Select(a => new
                    {
                        JobId = a.Job.Id,
                        JobTitle = a.Job.Title,
                        AppliedOn = a.AppliedOn,
                        Company = a.Job.CompanyName
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (applicant == null)
                return NotFound("Applicant not found");

            return Ok(applicant);
        }
    }

    public class CreateJobDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string CompanyName { get; set; }
    }
}