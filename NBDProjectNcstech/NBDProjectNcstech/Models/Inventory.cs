using System.ComponentModel.DataAnnotations;

namespace NBDProjectNcstech.Models
{
    public class Inventory
    {
        public int ID { get; set; }


        [Display(Name = "Name")]
        [Required(ErrorMessage = "You cannot leave the Name Blank")]
        [StringLength(50, ErrorMessage = "Name cannot be more than 50 characters long.")]
        public string Name { get; set; }
		[Display(Name = "Description")]
		[StringLength(50, ErrorMessage = "Description  cannot be more than 250 characters long.")]
		public string Description { get; set; }

		[Display(Name = "Size")]
		public string Size { get; set; }

		[Display(Name = "Cost Price")]
		[Required(ErrorMessage = "You cannot leave the cost price Blank")]
		public decimal CostPrice { get; set; }
		
		public int ItemTypeID { get; set; }

        public ItemType ItemType { get; set; }
		public int UnitID {  get; set; }
		public Unit Unit { get; set; }

    }
}
