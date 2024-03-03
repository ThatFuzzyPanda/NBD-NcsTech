using System.ComponentModel.DataAnnotations;

namespace NBDProjectNcstech.Models
{
    public class ApprovalAudit: IApproval
    {
        [ScaffoldColumn(false)]
        [StringLength(256)]
        public string CreatedBy { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? CreatedOn { get; set; }

        [ScaffoldColumn(false)]
        [StringLength(256)]
        public string UpdatedBy { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? UpdatedOn { get; set; }

        [ScaffoldColumn(false)]
        [StringLength(256)]
        public string ApprovedBy { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? ApprovedOn { get; set; }

        [ScaffoldColumn(false)]
        [StringLength(256)]
        public string RejectedBy { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? RejectedOn { get; set; }
    }
}
