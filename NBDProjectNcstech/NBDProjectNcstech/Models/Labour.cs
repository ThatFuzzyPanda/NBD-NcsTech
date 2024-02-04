using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace NBDProjectNcstech.Models
{
    public class Labour
    {
        public int ID { get; set; }

        [Display(Name = "Labor Type")]
        public string LabourType { get; set; }
        [Display(Name = "Labor Pirce")]
        public decimal LabourPrice { get; set; }
        [Display(Name = "Labor Cost")]
        public decimal LavourCost { get; set; }

        public int LabourID { get; set; }

        public Labour labour { get; set; }
    }
}
