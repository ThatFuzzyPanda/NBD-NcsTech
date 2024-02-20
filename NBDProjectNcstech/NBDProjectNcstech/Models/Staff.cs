using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace NBDProjectNcstech.Models
{
    public class Staff
    {
        public int ID { get; set; }

        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "You cannot leave full name blank.")]
        public string FullName { get; set; }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "You cannot leave the phone number blank.")]
        [RegularExpression("^\\d{10}$", ErrorMessage = "The phone number must be exactly 10 numeric digits.")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10)]
        public string Phone {  get; set; }

        //Foreign Keys
        [Display(Name = "Position")]
        [Required(ErrorMessage = "Staff member must have a position.")]
        public int StaffPositionID { get; set; }
        public StaffPosition StaffPosition  { get; set; }

        [Display(Name = "Design Bid Staff")]
        public ICollection<DesignBidStaff> DesignBidStaffs { get; set; } = new HashSet<DesignBidStaff>();

    }
}
