using System.ComponentModel.DataAnnotations;

namespace NBDProjectNcstech.Models
{
    public class DesignBidStaff
    {
        [Display(Name = "Design Bid")]
        public int DesignBidID { get; set; }
        public DesignBid DesignBid { get; set; }

        [Display(Name = "Staff")]
        public int StaffID { get; set; }
        public Staff Staff { get; set; }
    }
}
