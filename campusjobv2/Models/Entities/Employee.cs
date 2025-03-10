using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace campusjobv2.Models.Entities
{
    public class Employee
    {
        [Key]
        public int Student_ID { get; set; }
        
        [Required]
        public int Recruitment_ID { get; set; }
        
        [ForeignKey("Recruitment_ID")]
        public virtual Recruiter Recruiter { get; set; } = null!;
        
        // Navigation properties
        public virtual ICollection<RightToWorkDocument> RightToWorkDocuments { get; set; } = new List<RightToWorkDocument>();
        public virtual ICollection<VisaStatus> VisaStatuses { get; set; } = new List<VisaStatus>();
        public virtual ICollection<OfferedShift> OfferedShifts { get; set; } = new List<OfferedShift>();
    }
}
