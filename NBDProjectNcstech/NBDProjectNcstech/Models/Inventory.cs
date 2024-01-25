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

        public int TypeID { get; set; }

        public ItemType ItemType { get; set; }

    }
}
