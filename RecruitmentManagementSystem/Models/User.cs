using System.ComponentModel.DataAnnotations;

namespace RecruitmentManagementSystem.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Address { get; set; }

        [Required]
        public UserType UserType { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string ProfileHeadline { get; set; }

        // Navigation properties
        public virtual Profile Profile { get; set; }
        public virtual ICollection<Job> PostedJobs { get; set; }
        public virtual ICollection<JobApplication> Applications { get; set; }
    }

    public enum UserType
    {
        Applicant,
        Admin
    }
}