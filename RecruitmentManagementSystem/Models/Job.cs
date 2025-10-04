namespace RecruitmentManagementSystem.Models
{
    public class Job
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PostedOn { get; set; } = DateTime.UtcNow;
        public int TotalApplications { get; set; }
        public string CompanyName { get; set; }

        // Foreign keys
        public int PostedById { get; set; }
        public virtual User PostedBy { get; set; }

        // Navigation
        public virtual ICollection<JobApplication> Applications { get; set; }
    }
}