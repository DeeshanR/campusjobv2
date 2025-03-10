namespace CampusJobs.Models
{
    public class TimeSheetsViewModel
    {
        public List<StudentTimeSheets> ConfirmedShifts { get; set; } = new List<StudentTimeSheets>();
        public List<StudentTimeSheets> AvailableShifts { get; set; } = new List<StudentTimeSheets>();
    }
}