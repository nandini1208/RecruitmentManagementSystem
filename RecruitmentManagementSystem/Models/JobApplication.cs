namespace RecruitmentManagementSystem.Models
{
    public class JobApplication
    {
        public int Id { get; set; }
        public DateTime AppliedOn { get; set; } = DateTime.UtcNow;

        // Foreign keys
        public int ApplicantId { get; set; }
        public virtual User Applicant { get; set; }

        public int JobId { get; set; }
        public virtual Job Job { get; set; }
    }
}