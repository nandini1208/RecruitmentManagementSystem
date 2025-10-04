namespace RecruitmentManagementSystem.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public string? ResumeFileAddress { get; set; }
        public string? Skills { get; set; }
        public string? Education { get; set; }
        public string? Experience { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

        // Foreign key
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}