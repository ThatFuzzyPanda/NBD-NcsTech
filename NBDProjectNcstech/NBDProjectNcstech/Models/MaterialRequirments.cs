using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace NBDProjectNcstech.Models
{
    public class MaterialRequirments
    {
        public int ID { get; set; }

        [Display(Name = "Quanity")]
        public int? Quanity { get; set; }
        [Display(Name = "Description")]
        [StringLength(50, ErrorMessage = "Description  cannot be more than 250 characters long.")]
        public string Description { get; set; }

        [Display(Name = "Length")]
        public int SizeL { get; set; }
        [Display(Name = "Width")]
        public int SizeW { get; set; }
        [Display(Name = "Height")]
        public int SizeH { get; set; }

        [Display(Name = "Cost Price")]
        [Required(ErrorMessage = "You cannot leave the cost price Blank")]
        public decimal CostPrice { get; set; }
        [Display(Name = "Sale Price")]
        [Required(ErrorMessage = "You cannot leave the Sale price Blank")]
        public decimal SalePrice { get; set; }

        public string Size()
        {
            return $"{SizeL} x {SizeW} x {SizeH}";
        }

        public int InventoryID { get; set; }

        public Inventory Inventory { get; set; }

    }
}
