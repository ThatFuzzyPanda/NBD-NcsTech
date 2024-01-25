using Microsoft.Identity.Client;

namespace NBDProjectNcstech.Models
{
    public class Labour
    {
        public int ID { get; set; } 

        public string LabourType { get; set; }

        public decimal LabourPrice { get; set; }

        public decimal LavourCost { get; set; }

        public int LabourID { get; set; }

        public Labour labour { get; set; }
    }
}
