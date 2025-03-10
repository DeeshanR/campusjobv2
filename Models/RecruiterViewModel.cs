namespace CampusJobs.Models
{
    public class RecruiterViewModel
    {
        public List<PendingShift> PendingShifts { get; set; } = new List<PendingShift>();
        public List<ActiveShift> ActiveShifts { get; set; } = new List<ActiveShift>();
    }

    public class PendingShift
    {
        public string ShiftId { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public bool IsVisaRestricted { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double ApprovedHours { get; set; }
        public string Duration { get; set; } = string.Empty;
    }

    public class ActiveShift
    {
        public string ShiftId { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}