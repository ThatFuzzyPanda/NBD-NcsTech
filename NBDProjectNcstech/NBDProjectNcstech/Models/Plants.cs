using System.ComponentModel.DataAnnotations;

namespace NBDProjectNcstech.Models
{
    public class Plants
    {
        public int ID { get; set; }
        [Display(Name = "Plant Name")]
        [Required(ErrorMessage = "You cannot Leave the Plant Name Blank")]
        [StringLength(50, ErrorMessage = "Plant name cannot be more than 50 characters long.")]
        public string Name { get; set; }
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
    }
}
