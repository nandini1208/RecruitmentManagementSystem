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
    public class ApplicantsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ApplicantsController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/applicants/uploadResume - Only Applicant users
        [HttpPost("uploadResume")]
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> UploadResume(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            // Check file type (only PDF and DOCX allowed)
            var allowedExtensions = new[] { ".pdf", ".docx" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
                return BadRequest("Only PDF and DOCX files are allowed");

            // Get current user
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _context.Users
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return Unauthorized();

            // Save file
            var fileName = $"resume_{userId}_{DateTime.UtcNow:yyyyMMddHHmmss}{fileExtension}";
            var filePath = Path.Combine("Resumes", fileName);

            // Create directory if not exists
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // ✅ THIRD-PARTY API INTEGRATION: Process resume and extract data
            var parsedData = await CallResumeParserAPI(file);

            // Update user profile with EXTRACTED data from third-party API
            if (user.Profile != null)
            {
                user.Profile.ResumeFileAddress = filePath;
                user.Profile.Skills = parsedData.Skills;
                user.Profile.Education = parsedData.Education;
                user.Profile.Experience = parsedData.Experience;
                user.Profile.Phone = parsedData.Phone;
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Resume uploaded and processed successfully",
                FilePath = filePath,
                ExtractedData = new
                {
                    parsedData.Skills,
                    parsedData.Education,
                    parsedData.Experience,
                    parsedData.Phone
                }
            });
        }

        // ✅ THIRD-PARTY API SIMULATION
        private async Task<ResumeData> CallResumeParserAPI(IFormFile file)
        {
            // Simulate API call delay and processing
            await Task.Delay(1000);

            // In a real scenario, this would call an actual resume parsing API like:
            // - Affinda API
            // - Sovren API  
            // - ResumeParser API
            // For this assignment, we simulate extracted data

            return new ResumeData
            {
                Skills = "C#, .NET Core, ASP.NET MVC, Entity Framework, SQL Server, Web API, JavaScript",
                Education = "Bachelor of Computer Science - University of Technology (2020-2024)\nGPA: 3.8/4.0",
                Experience = "Software Developer at Tech Solutions Inc. (2023-Present)\n- Developed web applications using .NET Core\n- Implemented RESTful APIs\n- Worked with SQL Server databases",
                Phone = "+1 (555) 123-4567"
            };
        }
    }

    // ✅ Resume Data Model for third-party API response
    public class ResumeData
    {
        public string Skills { get; set; }
        public string Education { get; set; }
        public string Experience { get; set; }
        public string Phone { get; set; }
    }
}