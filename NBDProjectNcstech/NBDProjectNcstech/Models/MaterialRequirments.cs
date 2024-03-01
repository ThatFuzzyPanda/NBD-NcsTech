using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace NBDProjectNcstech.Models
{
    public class MaterialRequirments
    {
        public int ID { get; set; }

        [Display(Name = "Quanity")]
        public int? Quanity { get; set; }
		[Display(Name = "Extended Price")]
		[Required(ErrorMessage = "You cannot leave the Sale price Blank")]
		public decimal ExtendedPrice { get; set; }

		//Foregin Keys
		[Display(Name = "Inventory")]
        public int InventoryID { get; set; }
        public Inventory Inventory { get; set; }

        [Display(Name = "Design Bid")]
        public int DesignBidID { get; set; }
        public DesignBid DesignBid { get; set; }

    }
}
