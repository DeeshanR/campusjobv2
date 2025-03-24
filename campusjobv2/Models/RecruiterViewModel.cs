using campusjobv2.Models.Entities;
using System.Collections.Generic;

namespace campusjobv2.Models
{
    public class RecruiterViewModel
    {
        public List<ShiftRecord> PendingShifts { get; set; } = new List<ShiftRecord>();
        public List<ShiftRecord> ActiveShifts { get; set; } = new List<ShiftRecord>();

        public class ShiftRecord
        {
            public int ShiftId { get; set; }
            public DateTime Date { get; set; }
            public int StudentId { get; set; }
            public string StudentName { get; set; }
            public bool IsVisaRestricted { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public decimal ApprovedHours { get; set; }
            public decimal Duration { get; set; }
        }
    }
}
