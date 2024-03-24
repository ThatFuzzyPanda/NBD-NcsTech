using System.ComponentModel.DataAnnotations;

namespace NBDProjectNcstech.Models
{
    public enum ApprovalStatus
    {
        Approved,
        Pending,
        Denied
    }

    public class Approval
    {
        public int ID { get; set; }

        [Display(Name = "Admin Approval Status")]
        public string AdminApprovalStatus { get; set; }

        [Display(Name = "Admin Approval Date")]
        [DataType(DataType.DateTime)]
        public DateTime AdminApprovalDate { get; set; }

        [Display(Name = "Admin Notes")]
        public string AdminApprovalNotes { get; set; }

        [Display(Name = "Client Approval Status")]
        public string ClientApprovalStatus { get; set; }

        [Display(Name = "Client Approval Date")]
        [DataType(DataType.DateTime)]
        public DateTime ClientApprovalDate { get; set; }

        [Display(Name = "Client Notes")]
        public string ClientApprovalNotes { get; set;}


        [Display(Name = "Design Bid")]
        public ICollection<DesignBid> DesignBids { get; set; } = new HashSet<DesignBid>();
    }
}
