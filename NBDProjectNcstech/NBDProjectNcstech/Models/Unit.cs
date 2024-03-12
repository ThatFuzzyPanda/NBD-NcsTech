using System.ComponentModel.DataAnnotations;

namespace NBDProjectNcstech.Models
{
    public class Unit
    {
        public int ID { get; set; }

        [Display(Name = "Unit")]
        [Required(ErrorMessage = "You cannot leave the Unit name blank.")]
        public string Name { get; set; }

        public ICollection<Inventory> Inventories { get; set; } = new HashSet<Inventory>();
        public ICollection<MaterialRequirments> MaterialRequirments { get; set; } = new HashSet<MaterialRequirments>();


    }
}
