using MedicalOffice.Models;
using System.ComponentModel.DataAnnotations;

namespace NBDProjectNcstech.Models
{
    public class DesignBid : ApprovalAudit
    {
        public int ID { get; set; }

        //Foreign Keys
        [Display(Name = "Project")]
        [Required(ErrorMessage = "Design Bid must have a Project")]
        public int ProjectID { get; set; }
        public Project Project { get; set; }

        //[Display(Name = "Approvals")]
        //public ICollection<Approval> Approvals { get; set; } = new HashSet<Approval>();
        [Display(Name = "Design Bid")]
        public int? ApprovalID { get; set; }
        public Approval Approval { get; set; }

        [Display(Name = "Labour Requirements")]
        public ICollection<LabourRequirments> LabourRequirments { get; set; } = new HashSet<LabourRequirments>();

        [Display(Name = "Material Requirments")]
        public ICollection<MaterialRequirments> MaterialRequirments { get; set; } = new HashSet<MaterialRequirments>();

        [Display(Name = "Design Bid Staff")]
        public ICollection<DesignBidStaff> DesignBidStaffs { get; set; } = new HashSet<DesignBidStaff>();
    }
}
