using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace campusjobv2.Models.Entities
{
    public class OfferedShift
    {
        [Key]
        public int Offer_ID { get; set; }
        
        [Required]
        public int Student_ID { get; set; }
        
        [Required]
        public int Recruitment_ID { get; set; }
        
        [Required]
        public DateTime Date_Offered { get; set; } = DateTime.Now;
        
        [Required]
        public bool Status { get; set; } // 0: Pending, 1: Accepted
        
        [Required]
        public DateTime Start_Date { get; set; }
        
        [Required]
        public DateTime End_Date { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Total_Hours { get; set; }
        
        [ForeignKey("Student_ID")]
        public virtual Employee Employee { get; set; } = null!;
        
        [ForeignKey("Recruitment_ID")]
        public virtual Recruiter Recruiter { get; set; } = null!;
        
        // Navigation properties
        public virtual ICollection<ApprovedShift> ApprovedShifts { get; set; } = new List<ApprovedShift>();
    }
}
