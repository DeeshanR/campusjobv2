namespace CampusJobs.Models
{
    public class AdminViewModel
    {
        public List<AuditAction> AuditBoard { get; set; } = new List<AuditAction>();
    }

    public class AuditAction
    {
        public string ShiftId { get; set; } = string.Empty;
        public string StudentId { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string Recruiter { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Duration { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}