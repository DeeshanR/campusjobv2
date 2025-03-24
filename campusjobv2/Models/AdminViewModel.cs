using campusjobv2.Models.Entities;
using System.Collections.Generic;

namespace campusjobv2.Models
{
    public class AdminViewModel
    {
        public List<AuditRecord> AuditBoard { get; set; } = new List<AuditRecord>();

        public class AuditRecord
        {
            public int ShiftId { get; set; }
            public int StudentId { get; set; }
            public string StudentName { get; set; }
            public string Recruiter { get; set; }
            public DateTime Date { get; set; }
            public decimal Duration { get; set; }
            public string Status { get; set; }
        }
    }
}
