using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace NBDProjectNcstech.Models
{
    public class MaterialRequirments
    {
        public string SummaryQuantity
        {
            get
            {
                return Quanity + " " + Unit;
            }
        }
        public int ID { get; set; }

        [Display(Name = "Quanity")]
        public int? Quanity { get; set; }
		[Display(Name = "Extended Price")]
        public decimal ExtendedPrice
        {
            get
            {
                if (Inventory != null)
                {
                    return (decimal)(Quanity ?? 0) * Inventory.CostPrice;
                }
                return 0;
            }
        }

        //Foregin Keys
        [Display(Name = "Inventory")]
        public int InventoryID { get; set; }
        public Inventory Inventory { get; set; }

        [Display(Name = "Design Bid")]
        public int DesignBidID { get; set; }
        public DesignBid DesignBid { get; set; }
        public int UnitID { get; set; }
        public Unit Unit { get; set; }

    }
}
