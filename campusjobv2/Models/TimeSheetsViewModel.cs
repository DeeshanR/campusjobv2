using campusjobv2.Models.Entities;
using System.Collections.Generic;

namespace campusjobv2.Models
{
    public class TimeSheetsViewModel
    {
        public List<ShiftInfo> ConfirmedShifts { get; set; } = new List<ShiftInfo>();
        public List<ShiftInfo> AvailableShifts { get; set; } = new List<ShiftInfo>();

        public class ShiftInfo
        {
            public DateTime Date { get; set; }
            public int ShiftId { get; set; }
            public string AvailableShifts { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public decimal TotalHours { get; set; }
            public string Recruiter { get; set; }
        }
    }
}
