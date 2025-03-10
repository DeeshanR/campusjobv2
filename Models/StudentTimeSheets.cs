namespace CampusJobs.Models
{
    public class StudentTimeSheets
    {
        public bool IsVisaRestricted { get; set; }
        public DateTime Date { get; set; }
        public string ShiftId { get; set; } = string.Empty;
        public string AvailableShifts { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double TotalHours { get; set; }
        public string Recruiter { get; set; } = string.Empty;
    }
}